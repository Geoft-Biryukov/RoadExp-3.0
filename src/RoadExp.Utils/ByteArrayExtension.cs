namespace RoadExp.Utils
{
    /// <summary>
    /// Расширение для работы с массивами байт
    /// </summary>
    public static class ByteArrayExtension
    {
        /// <summary>
        /// Поиск первой позиции последовательности в массиве байт
        /// </summary>
        /// <param name="src"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int Search(this byte[] src, byte[] pattern)
        {
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            for (int i = 0; i < maxFirstCharSlot; i++)
            {
                if (src[i] != pattern[0])
                    continue;
                for (int j = pattern.Length - 1; j >= 1; j--)
                {
                    if (src[i + j] != pattern[j])
                        break;
                    if (j == 1)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Поиск первой позиции указанного байта в массиве байт
        /// </summary>
        /// <param name="src"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static int Search(this byte[] src, byte pattern)
        {
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] != pattern)
                    return i;
            }
            return -1;
        }
    }
}
