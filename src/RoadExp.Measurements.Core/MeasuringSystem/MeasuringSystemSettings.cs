namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Представляет абстрактный базовый класс для конфигурационных настроек измерительной системы.
    /// </summary>
    public abstract class MeasuringSystemSettings
    {
        /// <summary>
        /// Получает или задает значение, указывающее, запущена ли система в режиме эмуляции (виртуального устройства).
        /// </summary>
        public bool IsEmulationMode { get; set; }

        /// <summary>
        /// Получает или задает имя файла, используемого для сохранения зарегистрированных измерительных данных.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Получает или задает путь к директории, в которой будет сохранен файл с измерительными данными.
        /// </summary>
        public string Path { get; set; }
    }
}
