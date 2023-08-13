namespace packetSniffer
{
    class ByteCalculator
    {
        const int kiloBite = 1024;

        private static int sizeT;

        public static string CalculateByteString(int size)
        {
            sizeT += size;
            if (sizeT < kiloBite)
                return $"{sizeT} B";
            else if (sizeT >= kiloBite && sizeT < (kiloBite * kiloBite))
            {
                return $"{sizeT / kiloBite} KB";
            }
            else
            {
                return $"{sizeT / kiloBite * kiloBite} MB";
            }
        }

        public static void ResetTotalSize()
        {
            sizeT = 0;
        }
    }
}
