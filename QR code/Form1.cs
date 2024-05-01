namespace QR_code
{
    public partial class Form1 : Form
    {
        byte[,] Number_of_blocks = {
           //0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 Errorcorrectionlevel
            {0,  1,  1,  2,  4,  4,  4,  5,  6,  8,  8, 11, 11, 16, 16, 18, 16, 19, 21, 25, 25, 25, 34, 30, 32, 35, 37, 40, 42, 45, 48, 51, 54, 57, 60, 63, 66, 70, 74, 77, 81}, // High
            {0,  1,  1,  2,  2,  4,  4,  6,  6,  8,  8,  8, 10, 12, 16, 12, 17, 16, 18, 21, 20, 23, 23, 25, 27, 29, 34, 34, 35, 38, 40, 43, 45, 48, 51, 53, 56, 59, 62, 65, 68}, // Quartile
            {0,  1,  1,  1,  2,  2,  4,  4,  4,  5,  5,  5,  8,  9,  9, 10, 10, 11, 13, 14, 16, 17, 17, 18, 20, 21, 23, 25, 26, 28, 29, 31, 33, 35, 37, 38, 40, 43, 45, 47, 49}, // Medium
            {0,  1,  1,  1,  1,  1,  2,  2,  2,  2,  4,  4,  4,  4,  4,  6,  6,  6,  6,  7,  8,  8,  9,  9, 10, 12, 12, 12, 13, 14, 15, 16, 17, 18, 19, 19, 20, 21, 22, 24, 25}  // Low
        };

        byte[,] Number_of_EC_bytes_per_block =
        {
           //0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 Errorcorrectionlevel
    		{0, 17, 28, 22, 16, 22, 28, 26, 26, 24, 28, 24, 28, 22, 24, 24, 30, 28, 28, 26, 28, 30, 24, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30},  // High
    		{0, 13, 22, 18, 26, 18, 24, 18, 22, 20, 24, 28, 26, 24, 20, 30, 24, 28, 28, 26, 30, 28, 30, 30, 30, 30, 28, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30}, //# Quartile
    		{0, 10, 16, 26, 18, 24, 16, 18, 22, 22, 26, 30, 22, 22, 24, 24, 28, 28, 26, 26, 26, 26, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28}, //# Medium
            {0,  7, 10, 15, 20, 26, 18, 20, 24, 30, 18, 20, 24, 26, 30, 22, 24, 28, 30, 28, 28, 28, 28, 30, 30, 26, 28, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30}  //# Low
        };

        //int[] Number_of_data_bits = { 0, 208, 359, 567, 807, 1079, 1383, 1568, 1936, 2336, 2768, 3232, 3728, 4256, 4651, 5243, 5867, 6523, 7211, 7931, 8683, 9252, 10068, 10916, 11796, 12708, 13652, 14628, 15371, 16411, 17483, 18587, 19723, 20891, 22091, 23008, 24272, 25568, 26896, 28256, 29648 };
        int[] Number_of_bits = { 0, 208, 352, 560, 800, 1072, 1376, 1568, 1936, 2336, 2768, 3232, 3728, 4256, 4648, 5240, 5864, 6520, 7208, 7928, 8680, 9248, 10064, 10912, 11792, 12704, 13648, 14624, 15368, 16408, 17480, 18584, 19720, 20888, 22088, 23008, 24272, 25568, 26896, 28256, 29648 };

        /// <summary>
        /// Get the version by number of bits in the data (including the error-corection section)
        /// </summary>
        /// <param name="n">The number of bits including the error-correction section</param>
        int GetVersionByNumberOfBits(int n)
        {
            if (n > 29648)
                return -1;

            int l = 1, h = 40;
            while (l != h - 1)
            {
                int m = (h + l) / 2;
                if (Number_of_bits[m] > n)
                    h = m;
                else
                    l = m;
            }
            if (n <= Number_of_bits[l])
                return l;
            return h;
        }

        /// <summary>
        /// Get the version by number of bits in the data (without the error-corection section)
        /// </summary>
        /// <param name="n">The number of bits without the error-correction section</param>
        /// <param name="ecl">The error-correction-level of the QR</param>
        int GetVersionByNumberOfBits(string text, ECL ecl, Encoding enc)
        {
            int n = text.Length;
            switch (enc)
            {
                case Encoding.Numeric:
                    n = n * 3 + n / 3; // NumberOfBits *= 4/3
                    if (n % 3 == 1)
                        n += 4;
                    else if (n % 3 == 2)
                        n += 7;
                    break;
                case Encoding.Alphanumeric:
                    n = n * 5 + n / 2 + (n & 1); // NumberOfBits *= 5.5 + numberOfBits % 2
                    break;
                case Encoding.ECI_UTF_16:
                    n = n * 16 + 12; // UTF-16 use 16 bits per a character, ECI + ECI_Assignment
                    break;
                case Encoding.Byte:
                    n *= 8;
                    break;
            }

            n += 8; // ENC + END

            if (n + (Number_of_blocks[(int)ecl, 40] * Number_of_EC_bytes_per_block[(int)ecl, 40] << 3) + GetNumberOfBitsInLengthField(40, enc) > 29648)
                return -1;

            int l = 1, h = 40;
            while (l != h - 1)
            {
                int m = (h + l) / 2;
                if (Number_of_bits[m] > n + (Number_of_blocks[(int)ecl, m] * Number_of_EC_bytes_per_block[(int)ecl, m] << 3) + GetNumberOfBitsInLengthField(m, enc))
                    h = m;
                else
                    l = m;
            }
            if (n + (Number_of_blocks[(int)ecl, l] * Number_of_EC_bytes_per_block[(int)ecl, l] << 3) + GetNumberOfBitsInLengthField(l, enc) <= Number_of_bits[l])
                return l;
            return h;
        }

        static int GetNumberOfBitsInLengthField(int version, Encoding encoding)
        {
            // Version:     1-9   10-26   27-40
            // Numeric:      10    12       14
            // Alphanumeric: 9     11       13
            // Byte:         8     16       16


            if (version <= 9)
            {
                switch (encoding)
                {
                    case Encoding.Numeric:
                        return 10;
                    case Encoding.Alphanumeric:
                        return 9;
                    case Encoding.ECI_UTF_16:
                    case Encoding.Byte:
                        return 8;
                }
            }
            else if (version <= 26)
            {
                switch (encoding)
                {
                    case Encoding.Numeric:
                        return 12;
                    case Encoding.Alphanumeric:
                        return 11;
                    case Encoding.ECI_UTF_16:
                    case Encoding.Byte:
                        return 16;
                }
            }
            else
            {
                switch (encoding)
                {
                    case Encoding.Numeric:
                        return 14;
                    case Encoding.Alphanumeric:
                        return 13;
                    case Encoding.ECI_UTF_16:
                    case Encoding.Byte:
                        return 16;
                }
            }
            return -1;
        }

        byte[] Matrix_size = { 0, 21, 25, 29, 33, 37, 41, 45, 49, 53, 57, 61, 65, 69, 73, 77, 81, 85, 89, 93, 97, 101, 105, 109, 113, 117, 121, 125, 129, 133, 137, 141, 145, 149, 153, 157, 161, 165, 169, 173, 177 };

        byte[][] Alignment_position = {
            null, // 0
            new byte[] { }, // 1
            new byte[] { 6, 18 }, // 2
            new byte[] { 6, 22 }, // 3
            new byte[] { 6, 26 }, // 4
            new byte[] { 6, 30 }, // 5
            new byte[] { 6, 34 }, // 6
            new byte[] { 6, 22, 38 }, // 7
            new byte[] { 6, 24, 42 }, // 8
            new byte[] { 6, 26, 46 }, // 9
            new byte[] { 6, 28, 50 }, // 10
            new byte[] { 6, 30, 54 }, // 11
            new byte[] { 6, 32, 58 }, // 12
            new byte[] { 6, 34, 62 }, // 13
            new byte[] { 6, 26, 46, 66 }, // 14
            new byte[] { 6, 26, 48, 70 }, // 15
            new byte[] { 6, 26, 50, 74 }, // 16
            new byte[] { 6, 30, 54, 78 }, // 17
            new byte[] { 6, 30, 56, 82 }, // 18
            new byte[] { 6, 30, 58, 86 }, // 19
            new byte[] { 6, 34, 62, 90 }, // 20
            new byte[] { 6, 28, 50, 72, 94 }, // 21
            new byte[] { 6, 26, 50, 74, 98 }, // 22
            new byte[] { 6, 30, 54, 78, 102 }, // 23
            new byte[] { 6, 28, 54, 80, 106 }, // 24
            new byte[] { 6, 32, 58, 84, 110 }, // 25
            new byte[] { 6, 30, 58, 86, 114 }, // 26
            new byte[] { 6, 34, 62, 90, 118 }, // 27
            new byte[] { 6, 26, 50, 74, 98, 122 }, // 28
            new byte[] { 6, 30, 54, 78, 102, 126 }, // 29
            new byte[] { 6, 26, 52, 78, 104, 130 }, // 30
            new byte[] { 6, 30, 56, 82, 108, 134 }, // 31
            new byte[] { 6, 34, 60, 86, 112, 138 }, // 32
            new byte[] { 6, 30, 58, 86, 114, 142 }, // 33
            new byte[] { 6, 34, 62, 90, 118, 146 }, // 34
            new byte[] { 6, 30, 54, 78, 102, 126, 150 }, // 35
            new byte[] { 6, 24, 50, 76, 102, 128, 154 }, // 36
            new byte[] { 6, 28, 54, 80, 106, 132, 158 }, // 37
            new byte[] { 6, 32, 58, 84, 110, 136, 162 }, // 38
            new byte[] { 6, 26, 54, 82, 110, 138, 166 }, // 39
            new byte[] { 6, 30, 58, 86, 114, 142, 170 }, // 40
        };

        /// <summary>
        /// This class representing a bits-stream.
        /// The bits-stream is represented by an Int32 array.
        /// </summary>
        class BitsStream
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

        enum ECL
        {
            H = 0,
            Q,
            M,
            L
        }

        enum Encoding
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            ECI_UTF_16 = 0x7194, // 0111 00011001 0100
        }

        enum Mask
        {
            Automat = -1,
            flower = 0,
            rectangles,
            Pazzle,
            XoredRectangles,
            row2,
            checkers,
            diagonal3,
            column3,
        }

        public Form1()
        {
            InitializeComponent();

            MaskComboBox.SelectedIndex = 0;
            MaskComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            CorrectionLevel.SelectedIndex = 0;
            CorrectionLevel.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

            int m = pictureBox.Width;
            if (m > pictureBox.Height)
                m = pictureBox.Height;
            pictureBox.Image = new Bitmap(m, m);
        }

        // Assume correct parameters, i.e. x,y < m.size & x+w,y+h < m.size & w,h != 0
        // if x or y are negative the meaning is size - x or size - y
        void DrawRect(ref bool[,] m, int x, int y, int w, int h, bool v = true)
        {
            if (x < 0)
                x += m.GetLength(0);
            if (y < 0)
                y += m.GetLength(0);

            for (int i = x; i < x + w; i++)
            {
                m[i, y + h - 1] = v;
                m[i, y] = v;
            }

            for (int i = y; i < y + h; i++)
            {
                m[x + w - 1, i] = v;
                m[x, i] = v;
            }
        }

        // Assume correct parameters, i.e. x,y < m.size & x+w,y+h < m.size & w,h != 0
        // if x or y are negative the meaning is size - x or size - y
        void FillRect(ref bool[,] m, int x, int y, int w, int h, bool v = true)
        {
            if (x < 0)
                x += m.GetLength(0);
            if (y < 0)
                y += m.GetLength(0);

            for (int i = x; i < x + w; i++)
                for (int j = y; j < y + h; j++)
                {
                    m[i, j] = v;
                }
        }

        List<byte> ReedSolomonComputeDivisor(int degree)
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

        byte ReedSolomonMultiply(byte x, byte y)
        {
            int z = 0;

            for (int i = 7; i >= 0; i--)
            {
                z = (z << 1) ^ ((z >> 7) * 0x11D);
                z ^= ((y >> i) & 1) * x;
            }
            return (byte)z;
        }

        List<byte> ReedSolomonAlgorithm(List<byte> data, List<byte> divisor)
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

        BitsStream AddDataErrorCorrection(BitsStream data, ECL ecl, int version)
        {
            int numberOfBlocks = Number_of_blocks[(int)ecl, version]; // How many blocks are there.
            int dataBytesInBlock = data.Count / 8 / numberOfBlocks; // How many bytes are there in the data section.
            int ECBytesInBlock = Number_of_EC_bytes_per_block[(int)ecl, version]; // How many bytes are there in the error-correction section.
            int bytesInBlock = dataBytesInBlock + ECBytesInBlock; // How many bytes are there in a block (data + error-correction).
            int numberOfLongBlocks = Number_of_bits[version] / 8 % numberOfBlocks; // Sometimes the bytes of the QR aren't divided evenly into the blocks - in those cases there're some long blocks (blocks whose length is bigger of the others by 1), those blocks are the ones at end.

            List<byte> RSdiv = ReedSolomonComputeDivisor(ECBytesInBlock);
            List<byte>[] blocksData = new List<byte>[numberOfBlocks];
            List<byte>[] blocksEC = new List<byte>[numberOfBlocks];
            // Calculate the error-correction for each block
            int dataIndex = 0;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                blocksData[i] = new List<byte>(dataBytesInBlock + (i < numberOfBlocks - numberOfLongBlocks ? 0 : 1));
                for (int j = 0; j < blocksData[i].Capacity; j++)
                    blocksData[i].Add(data.GetByte(dataIndex++));

                blocksEC[i] = ReedSolomonAlgorithm(blocksData[i], RSdiv);
            }

            //  Return the data + the error-corraction
            BitsStream res = new BitsStream(numberOfBlocks * bytesInBlock);
            // Add the data blocks to the result, byte from each block at a time
            for (int i = 0; i < dataBytesInBlock; i++)
                for (int j = 0; j < numberOfBlocks; j++)
                    res.AddByte(blocksData[j][i]);
            // Add one more byte for each of the long-blocks
            for (int i = numberOfBlocks - numberOfLongBlocks; i < numberOfBlocks; i++)
                res.AddByte(blocksData[i][^1]);
            // Add the error-correction blocks to the result, byte from each block at a time
            for (int i = 0; i < ECBytesInBlock; i++)
                for (int j = 0; j < numberOfBlocks; j++)
                    res.AddByte(blocksEC[j][i]);
            return res;
        }

        // Get a string text and an encoding format, return the data for the QR
        BitsStream GetData(Encoding encoding, string text, ECL ecl)
        {
            int version = GetVersionByNumberOfBits(text, ecl, encoding);
            if (version == -1)
                return null;

            int dataSize = Number_of_bits[version] - Number_of_EC_bytes_per_block[(int)ecl, version] * Number_of_blocks[(int)ecl, version] * 8;

            BitsStream data = new BitsStream(Number_of_bits[version]);

            // Push encoding
            if ((int)encoding < 16)
                data.AddBits((int)encoding, 4);
            else
                data.AddBits((int)encoding, 16); // Push ECI encoding

            // Push Length
            if (encoding == Encoding.ECI_UTF_16)
                data.AddBits(text.Length << 1, GetNumberOfBitsInLengthField(version, encoding));
            else
                data.AddBits(text.Length, GetNumberOfBitsInLengthField(version, encoding));

            // Push text
            switch (encoding)
            {
                case Encoding.Numeric:
                    for (int i = 0; i < text.Length; i += 3)
                    {
                        int d = 0;
                        d += text[i] - '0';
                        if (i + 1 >= text.Length)
                        {
                            data.AddBits(d, 4);
                            break;
                        }
                        d = d * 10 + text[i + 1] - '0';
                        if (i + 2 >= text.Length)
                        {
                            data.AddBits(d, 7);
                            break;
                        }
                        d = d * 10 + text[i + 2] - '0';

                        data.AddBits(d, 10);
                    }
                    break;
                case Encoding.Alphanumeric:
                    for (int i = 0; i < text.Length; i += 2)
                    {
                        int d = 0;
                        d += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:".IndexOf(text[i]);
                        if (i + 1 >= text.Length)
                        {
                            data.AddBits(d, 6);
                            break;
                        }
                        d = d * 45 + "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:".IndexOf(text[i + 1]);
                        data.AddBits(d, 11);
                    }
                    break;
                case Encoding.Byte:
                    for (int i = 0; i < text.Length; i++)
                    {
                        byte d = (byte)text[i];
                        data.AddByte(d);
                    }
                    break;
                case Encoding.ECI_UTF_16:
                    for (int i = 0; i < text.Length; i++)
                        data.AddBits(text[i], 16);
                    break;
            }

            // Push END
            data.AddBits(0, 4);

            // Padding
            while ((dataSize - data.Count) / 32 > 1)
                data.AddInteger(0);
            while ((dataSize - data.Count) / 8 > 1)
                data.AddByte(0);
            while (data.Count < dataSize)
                data.AddBit(0);

            return AddDataErrorCorrection(data, ecl, version);
        }

        // return the data orgenized in the matrix
        // ** The return matrix doesn't include the Format Information **
        bool[,] EncodeDataIntoMatrix(int version, BitsStream data)
        {
            int size = Matrix_size[version];
            bool[,] matrix = new bool[size, size];
            byte[] alignmentPos = Alignment_position[version];

            // Alignment
            for (int i = 0; i < alignmentPos.Length; i++)
                for (int j = 0; j < alignmentPos.Length; j++)
                {
                    if (i == 0 && (j == 0 || j == alignmentPos.Length - 1) || j == 0 && i == alignmentPos.Length - 1)
                        continue;
                    DrawRect(ref matrix, alignmentPos[i] - 2, alignmentPos[j] - 2, 5, 5);
                    matrix[alignmentPos[i], alignmentPos[j]] = true;
                }

            // Fixed Patterns
            DrawRect(ref matrix, 0, 0, 7, 7);
            DrawRect(ref matrix, 0, -7, 7, 7);
            DrawRect(ref matrix, -7, 0, 7, 7);
            FillRect(ref matrix, 2, 2, 3, 3);
            FillRect(ref matrix, 2, -5, 3, 3);
            FillRect(ref matrix, -5, 2, 3, 3);
            for (int i = 8; i <= size - 9; i++)
                matrix[i, 6] = i % 2 == 0;
            for (int i = 8; i <= size - 9; i++)
                matrix[6, i] = i % 2 == 0;
            matrix[8, size - 8] = true;


            // Encode the data to the matrix (with a respect to QR code)
            int index = 0;
            int Iy = size - 1, Ix = size - 1;
            while (index < data.Count)
            {
                // up
                while (Iy > 8 || (Iy >= 0 && 8 < Ix && Ix < size - 8))
                {
                    // Jump over the fixed patterns
                    if (Iy == 6)
                        Iy = 5;

                    // Jump over version information, when [ version >= 7 ]
                    if (version >= 7 && Iy == 5 && Ix == size - 9)
                    {
                        Iy = 0;
                        Ix = size - 12;
                        for (int i = 0; i < 6 && index < data.Count; i++)
                            matrix[Ix, Iy++] = data[index++];

                        Ix += 3; // Prepear for the next steps
                        break;
                    }

                    // Deal with alignment
                    if (!(Iy == 8 && Ix == size - 9) && !(Ix == 8 && Iy == size - 16) && alignmentPos.Any(b => b + 2 == Iy))
                    {
                        if (alignmentPos.Any(b => b - 1 <= Ix && Ix <= b + 3)) // Is the left side blocked?
                        {
                            if (alignmentPos.Any(b => b - 2 <= Ix && Ix <= b + 2)) // Is the right side blocked?
                                Iy -= 5; // Jump over alignment patterns
                            else
                                // Fill 5 bits in the right side
                                for (int i = 0; i < 5 && index < data.Count; i++)
                                {
                                    if (Iy == 6) // Jump over the fixed patterns
                                    {
                                        Iy = 5;
                                        i++;
                                    }
                                    matrix[Ix, Iy--] = data[index++];
                                }
                        }
                        else if (alignmentPos.Any(b => b - 2 <= Ix && Ix <= b + 2)) // Is the right side blocked?
                            // Fill 5 bits in the left side
                            for (int i = 0; i < 5 && index < data.Count; i++)
                            {
                                if (Iy == 6) // Jump over the fixed patterns
                                {
                                    Iy = 5;
                                    i++;
                                }
                                matrix[Ix - 1, Iy--] = data[index++];
                            }
                    }
                    matrix[Ix, Iy] = data[index];
                    Ix--;
                    if (++index >= data.Count)
                        break;
                    matrix[Ix, Iy] = data[index];
                    Iy--;
                    Ix++;
                    if (++index >= data.Count)
                        break;
                }
                // Prepear for the next steps
                Ix -= 2;
                Iy++;

                // Jump over the fixed patterns
                if (Ix == 6)
                    Ix = 5;

                // down
                while (index < data.Count && (Iy < size - 8 || (Iy < size && Ix > 8)))
                {
                    // Jump over the fixed patterns
                    if (Iy == 6)
                        Iy = 7;

                    // Jump over version information, when [ version >= 7 ]
                    if (version >= 7 && Ix == 5 && Iy == size - 11)
                        break;

                    // Deal with alignment
                    if (!(Ix == 5 && Iy == size - 9) && alignmentPos.Any(b => b - 2 == Iy))
                    {
                        if (alignmentPos.Any(b => b - 1 <= Ix && Ix <= b + 3)) // Is the left side blocked?
                        {
                            if (alignmentPos.Any(b => b - 2 <= Ix && Ix <= b + 2)) // Is the right side blocked?
                                Iy += 5; // Jump over alignment patterns
                            else
                                // Fill 5 bits in the right side
                                for (int i = 0; i < 5 && index < data.Count; i++)
                                {
                                    if (Iy == 6) // Jump over the fixed patterns
                                    {
                                        Iy = 7;
                                        i++;
                                    }
                                    matrix[Ix, Iy++] = data[index++];
                                }
                        }
                        else if (alignmentPos.Any(b => b - 2 <= Ix && Ix <= b + 2)) // Is the right side blocked?
                            // Fill 5 bits in the left side
                            for (int i = 0; i < 5 && index < data.Count; i++)
                            {
                                if (Iy == 6) // Jump over the fixed patterns
                                {
                                    Iy = 7;
                                    i++;
                                }
                                matrix[Ix - 1, Iy++] = data[index++];
                            }
                    }

                    matrix[Ix, Iy] = data[index];
                    Ix--;
                    if (++index >= data.Count)
                        break;
                    matrix[Ix, Iy] = data[index];
                    Iy++;
                    Ix++;
                    if (++index >= data.Count)
                        break;
                }
                // Prepear for the next steps
                Ix -= 2;
                Iy--;

                if (Ix == 8) // Jump over the fixed patterns
                    Iy = size - 9;
            }

            return matrix;
        }

        // Get a matrix of QR code, choose a mask & add it to the matrix
        Mask AddMask(int version, ref bool[,] m, Mask mask)
        {
            byte[] alignmentPos = Alignment_position[version];

            int size = m.GetLength(0);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (i != 6 && j != 6 && ((i > 8 && j > 8) || (i > 8 && i < size - 8) || (j > 8 && j < size - 8)) // Isn't (j, i) in the fixed pattern region?
                        && !(alignmentPos.Any(b => b - 2 <= i && i <= b + 2) && alignmentPos.Any(b => b - 2 <= j && j <= b + 2)) // Isn't (j, i) in the alignment pattern region?
                        && (version < 7 || !(j <= 5 && i >= size - 11 || i <= 5 && j >= size - 11))) // When version >= 7, don't put the mask on the version information sections
                    {
                        switch (mask)
                        {
                            case Mask.Automat:
                            case Mask.flower:
                                m[j, i] ^= i * j % 2 + i * j % 3 == 0;
                                break;
                            case Mask.rectangles:
                                m[j, i] ^= (i / 2 + j / 3) % 2 == 0;
                                break;
                            case Mask.Pazzle:
                                m[j, i] ^= (i * j % 3 + i + j) % 2 == 0;
                                break;
                            case Mask.XoredRectangles:
                                m[j, i] ^= (i * j % 3 + i * j) % 2 == 0;
                                break;
                            case Mask.row2:
                                m[j, i] ^= i % 2 == 0;
                                break;
                            case Mask.checkers:
                                m[j, i] ^= (i + j) % 2 == 0;
                                break;
                            case Mask.diagonal3:
                                m[j, i] ^= (i + j) % 3 == 0;
                                break;
                            case Mask.column3:
                                m[j, i] ^= j % 3 == 0;
                                break;
                        }
                    }

            if (mask == Mask.Automat)
                return Mask.flower;
            return mask;
        }

        /// <summary>
        /// Calculate the (a,b)BCH code in respect to gx;
        /// Notice: gx, max degree is 31 and 0 < a,b < 32
        /// </summary>
        int BCH(int gx, int data, int a)
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

        int GetFormatErrorCorrection(int format)
        {
            return BCH(0b10100110111, format, 15); // (15,5)BCH
        }

        int GetVersionErrorCorrection(int version)
        {
            return BCH(0b1111100100101, version, 18); // (18,6)BCH
        }

        void AddFormat(ref bool[,] m, Mask mask, ECL ecl)
        {
            int size = m.GetLength(0);

            int format = GetFormatErrorCorrection(((int)ecl << 3 | (int)mask) ^ 0b10101) ^ 0b0101010000010010;

            // Write to up-left corner
            for (int i = 0; i < 6; i++)
                m[8, i] = (format & (1 << i)) != 0;
            m[8, 7] = (format & (1 << 6)) != 0;
            m[8, 8] = (format & (1 << 7)) != 0;
            m[7, 8] = (format & (1 << 8)) != 0;
            for (int i = 0; i < 6; i++)
                m[5 - i, 8] = (format & (1 << 9 + i)) != 0;

            // Write to the two other corners
            for (int i = 0; i < 8; i++)
                m[size - 1 - i, 8] = (format & (1 << i)) != 0;
            for (int i = 0; i < 7; i++)
                m[8, size - 7 + i] = (format & (1 << 8 + i)) != 0;

        }

        void AddVersionInformation(ref bool[,] m, int version)
        {
            int VIEC = GetVersionErrorCorrection(version);
            int VIEC_copy = VIEC;
            int size = m.GetLength(0);

            // Write to up-right corner
            for (int i = 0; i < 6; i++)
                for (int j = size - 11; j < size - 8; j++)
                {
                    m[j, i] = (VIEC & 1) == 1;
                    VIEC >>= 1;
                }

            // Write to bottom-left corner
            for (int i = 0; i < 6; i++)
                for (int j = size - 11; j < size - 8; j++)
                {
                    m[i, j] = (VIEC_copy & 1) == 1;
                    VIEC_copy >>= 1;
                }
        }

        Encoding ChooseEncoding(string text)
        {
            Encoding encoding = Encoding.ECI_UTF_16; // Default
            if (text.All(c => '0' <= c && c <= '9')) // Numeric
                encoding = Encoding.Numeric;
            else if (text.All(c => ('0' <= c && c <= '9') || ('A' <= c && c <= 'Z') || " $%*+-./:".Contains(c))) // Alphanumeric
                encoding = Encoding.Alphanumeric;
            else if (text.All(c => (char)((byte)c) == c))
                encoding = Encoding.Byte;
            return encoding;
        }

        Brush white = new SolidBrush(Color.White);
        Brush black = new SolidBrush(Color.Black);
        void UpdateQR()
        {
            string text = InputTextBox.Text.Substring(0);
            ECL ecl = 3 - (ECL)CorrectionLevel.SelectedIndex;
            Mask mask = (Mask)MaskComboBox.SelectedIndex - 1;

            // Clear the screen
            if (text.Length == 0)
            {
                labelVersion.Text = "";
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.Clear(Color.Transparent);
                graphics.Dispose();
                pictureBox.Refresh();
                return;
            }

            // Choose encoding
            Encoding encoding = ChooseEncoding(text);

            BitsStream data = GetData(encoding, text, ecl);
            if (data == null)
            {
                Graphics Graph = Graphics.FromImage(pictureBox.Image);
                unchecked { Graph.Clear(Color.FromArgb((int)0xffffe0e0)); }
                Graph.DrawString("Error - QR-codes can't contain the wanted amount of data", new Font("Aptos", 35, FontStyle.Regular), new SolidBrush(Color.Red), pictureBox.Bounds);
                Graph.Dispose();
                pictureBox.Refresh();
                labelVersion.Text = "ERROR";
                return;
            }

            int version = GetVersionByNumberOfBits(data.Count);
            bool[,] matrix = EncodeDataIntoMatrix(version, data);
            int size = matrix.GetLength(0);

            mask = AddMask(version, ref matrix, mask);

            AddFormat(ref matrix, mask, ecl);

            if (version >= 7)
                AddVersionInformation(ref matrix, version);

            // Draw into the screen
            Graphics G = Graphics.FromImage(pictureBox.Image);
            G.Clear(Color.Transparent);
            int ratio = pictureBox.Image.Width / size;
            G.FillRectangle(white, 0, 0, ratio * size, ratio * size);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j])
                        G.FillRectangle(black, i * ratio, j * ratio, ratio, ratio);
                }
            G.Dispose();
            labelVersion.Text = $"Version {version}";
            pictureBox.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateQR();
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateQR();
        }
 
        private void Expand_TextBox(object sender, EventArgs e)
        {
            if ((sender as Label).Text == "\x21f2") // Expand
            {
                InputTextBox.Size += new Size(0, 500);
                InputTextBox.BorderStyle = BorderStyle.FixedSingle;
                (sender as Label).Text = "\x21f1";
            }
            else // Contract
            {
                InputTextBox.Size -= new Size(0, 500);
                InputTextBox.BorderStyle = BorderStyle.None;
                (sender as Label).Text = "\x21f2";
            }
        }
    }
}
