namespace RoadExp.Utils
{
    /// <summary>
    /// Битовые операции с числом
    /// </summary>
    public static class BitHelpers
    {
        /// <summary>
        /// Установить бит
        /// </summary>
        /// <param name="input">Исходное число</param>
        /// <param name="bit">Порядковый номер бита который надо установить</param>
        /// <returns>Итоговое число</returns>
        public static ushort SetBit(ushort input, byte bit)
        {
            return (ushort)(input | (1 << bit));
        }

        /// <summary>
        /// Снять бит
        /// </summary>
        /// <param name="input">Исходное число</param>
        /// <param name="bit">Порядковый номер бита который надо снять</param>
        /// <returns>Итоговое число</returns>
        public static ushort ClearBit(ushort input, byte bit)
        {
            return (ushort)(input & ~(1 << bit));
        }
    }
}
