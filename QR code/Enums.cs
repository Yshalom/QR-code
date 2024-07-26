namespace QR_code
{
    internal enum ECL
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

}
