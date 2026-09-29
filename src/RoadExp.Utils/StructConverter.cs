using System.Runtime.InteropServices;

namespace RoadExp.Utils
{
    public static class StructConverter
    {
        /// <summary>
        /// Конвертирует структуру в массив байтов
        /// </summary>
        /// <typeparam name="T">Тип конвертируемой структуры</typeparam>
        /// <param name="value">Конвертируемая структура</param>
        /// <returns>Массив байтов, соответствующий структуре</returns>
        public static byte[] StructToByteArray<T>(T value) where T : struct
        {
            int size = Marshal.SizeOf(value);
            byte[] array = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(value, ptr, true);
                Marshal.Copy(ptr, array, 0, size);
                return array;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// Конвертирует массив байтов обратно в структуру.
        /// </summary>
        /// <typeparam name="T">Тип результирующей структуры</typeparam>
        /// <param name="bytes">Массив байтов</param>
        /// <returns>Заполненная структура указанного типа</returns>
        /// <exception cref="ArgumentException">Вызывается, если размер массива байт меньше размера структуры</exception>
        public static T ByteArrayToStruct<T>(byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            // Проверка безопасности: массив байт не должен быть меньше, чем размер самой структуры
            if (bytes == null || bytes.Length < size)
            {
                throw new ArgumentException($"Размер массива байт ({bytes?.Length ?? 0}) меньше, чем требуется для структуры {typeof(T).Name} ({size}).");
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                // Копируем байты из управляемого массива в выделенную unmanaged-память
                Marshal.Copy(bytes, 0, ptr, size);

                // Преобразуем unmanaged-память в готовую структуру
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                // Обязательно освобождаем память в блоке finally
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
