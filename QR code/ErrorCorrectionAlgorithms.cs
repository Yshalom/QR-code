using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_code
{
    internal static class ErrorCorrectionAlgorithms
    {
        public static List<byte> ReedSolomonComputeDivisor(int degree)
        {
            // start with the polynom ' x^0 '
            List<byte> result = new List<byte>(degree);
            for (int i = 0; i < degree - 1; i++)
                result.Add(0);
            result.Add(1);

            byte root = 1;
            for (int i = 0; i < degree; i++)
            {
                // Multiply the current product by (x - r^i)
                for (int j = 0; j < degree; j++)
                {
                    result[j] = ReedSolomonMultiply(result[j], root);
                    if (j != degree - 1)
                        result[j] ^= result[j + 1];
                }
                root = ReedSolomonMultiply(root, 0x02);
            }
            return result;
        }

        private static byte ReedSolomonMultiply(byte x, byte y)
        {
            int z = 0;

            for (int i = 7; i >= 0; i--)
            {
                z = (z << 1) ^ ((z >> 7) * 0x11D);
                z ^= ((y >> i) & 1) * x;
            }
            return (byte)z;
        }

        public static List<byte> ReedSolomonAlgorithm(List<byte> data, List<byte> divisor)
        {
            LinkedList<byte> result = new LinkedList<byte>();
            for (int i = 0; i < divisor.Count; i++)
                result.AddLast(0);
            foreach (byte b in data)
            {
                byte factor = (byte)(b ^ result.First());
                result.RemoveFirst();
                result.AddLast(0);
                LinkedListNode<byte> resultNode = result.First;
                for (int i = 0; i < divisor.Count; i++)
                {
                    resultNode.Value ^= ReedSolomonMultiply(divisor[i], factor);
                    resultNode = resultNode.Next;
                }
            }
            return new List<byte>(result);
        }

        /// <summary>
        /// Calculate the (a,b)BCH code in respect to gx;
        /// Notice: gx, max degree is 31 and 0 < a,b < 32
        /// </summary>
        public static int BCH(int gx, int data, int a)
        {
            int d_gx = 31;
            for (; d_gx >= 0 && (gx >> d_gx & 1) == 0; d_gx--) ; // Find the maximum degree of gx

            // Create px
            data <<= d_gx;
            int px = 0;
            for (int i = 0; i < a; i++)
                px |= (data >> i & 1) << i;

            // Calculate rx [ rx = px % gx mod 2 ]
            int rx = px;
            while (true)
            {
                // find rx degree
                int d_rx = a - 1;
                for (; d_rx >= d_gx && (rx >> d_rx & 1) == 0; d_rx--) ; // Find the maximum degree of rx
                if (d_rx < d_gx)
                    break;

                rx ^= gx << (d_rx - d_gx); // rx -= gx << d mod 2
            }

            // Return [ px - rx mod 2 ]
            return px ^ rx;


            //// The previous code - slow, the new one is faster.
            //// Create px
            //data <<= d_gx;
            //List<int> px = new List<int>(a);
            //for (int i = 0; i < a; i++)
            //    px.Add((data >> i) & 1);

            //// Calculate rx [ rx = px % gx ]
            //List<int> rx = new List<int>(px);
            //while (true)
            //{
            //    // find rx degree
            //    int d_rx = a - 1;
            //    for (; d_rx >= d_gx && rx[d_rx] == 0; d_rx--) ; // Find the maximum degree of rx
            //    if (d_rx < d_gx)
            //        break;

            //    // rx -= factor * gx
            //    int factor = rx[d_rx];
            //    d_rx -= d_gx;
            //    for (int i = 0; i <= d_gx; i++)
            //        if ((gx >> i & 1) != 0)
            //            rx[i + d_rx] -= factor;
            //}

            //// Copy [ px - rx ] to result (odd coefficient is 1, even is 0)
            //int res = 0;
            //for (int i = 14; i >= 0; i--)
            //    res = res * 2 + ((px[i] - rx[i]) & 1);

            //return res;
        }
    }
}
