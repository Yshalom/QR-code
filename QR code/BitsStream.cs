using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_code
{
    /// <summary>
    /// This class representing a bits-stream.
    /// The bits-stream is represented by an Int32 array.
    /// </summary>
    internal class BitsStream
    {
        private List<UInt32> data;
        public int Count { get; private set; }
        public int Capacity
        {
            get => data.Capacity;
        }

        public BitsStream(int capacity)
        {
            data = new List<UInt32>(capacity);
            Count = 0;
        }

        /// <param name="b">b is only 0 or 1</param>
        public void AddBit(int b)
        {
            uint _b = (uint)b;
            _b &= 1;
            if ((Count & 0x1f) == 0) // Count % 32 == 0
                data.Add(_b << 31);
            else
                data[^1] |= _b << (31 - Count % 32);
            Count++;
        }
        public void AddBit(bool T) => AddBit(T ? 1 : 0);

        /// <summary>
        /// Add n bits to the stream;
        /// if n % 32 is not 0, ignore the MSB-bits of d
        /// </summary>
        /// <param name="_d">The data to be added to the stream</param>
        /// <param name="n">How many bits to add (Maximum = 32, Minimum = 0)</param>
        public void AddBits(int d, int n)
        {
            uint _d = (uint)d;
            if (n == 0)
                return;
            if (n > 32)
                n = 32;

            _d <<= 32 - n & 0x1f; // Make the n-LSB-bits of d to the n-MSB
            int x = (32 - Count) & 0x1f;
            if (x != 0)
                data[^1] |= _d >> (32 - x);

            if (n - x > 0)
                data.Add(_d << x);

            Count += n;
        }
        public void AddInteger(int d) => AddBits(d, 32);
        public void AddByte(byte b) => AddBits(b, 8);

        /// <summary>
        /// Return the byte in the index i ( the bits of the result are in index i*8)
        /// </summary>
        /// <param name="i">Index</param>
        public byte GetByte(int i) => (byte)(data[i >> 2] >> (24 ^ ((i & 0x3) << 3))); // (byte)(data[i / 4] >> (24 - i % 4 * 8));

        public bool this[int i]
        {
            get => (data[i >> 5] & (1 << (0x1f ^ i & 0x1f))) != 0; // (data[i / 32] & (1 << (31 - i % 32))) != 0
        }
    }

}
