namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет контракт менеджера, отвечающего за сериализацию, десериализацию и инициализацию настроек измерительной системы.
    /// </summary>
    /// <typeparam name="T">Тип настроек измерительной системы, наследуемый от <see cref="MeasuringSystemSettings"/>.</typeparam>
    public interface IMeasuringSystemSettingsManager<T> where T : MeasuringSystemSettings
    {
        /// <summary>
        /// Загружает конфигурационные настройки из указанного файла.
        /// </summary>
        /// <param name="filePath">Путь к файлу конфигурации.</param>
        /// <returns>Экземпляр настроек типа <typeparamref name="T"/>, загруженный из файла.</returns>
        T Load(string fileName);

        /// <summary>
        /// Сохраняет текущие конфигурационные настройки в файл.
        /// </summary>
        /// <param name="settings">Экземпляр настроек для сохранения.</param>
        /// <param name="filePath">Путь к файлу, в который будут записаны настройки.</param>
        /// <returns>Значение <see langword="true"/>, если настройки успешно сохранены; иначе — <see langword="false"/>.</returns>
        bool Save(T settings, string filePath);

        /// <summary>
        /// Создает и возвращает экземпляр настроек, заполненный значениями по умолчанию.
        /// </summary>
        /// <returns>Новый экземпляр настроек типа <typeparamref name="T"/> со значениями по умолчанию.</returns>
        T GetDefaultSettings();

        /// <summary>
        /// Загружает настройки из файла, а в случае отсутствия файла или ошибки загрузки возвращает настройки по умолчанию.
        /// </summary>
        /// <param name="filePath">Путь к файлу конфигурации.</param>
        /// <returns>Загруженные из файла или созданные по умолчанию настройки типа <typeparamref name="T"/>.</returns>
        T LoadOrDefault(string filePath);
    }
}
