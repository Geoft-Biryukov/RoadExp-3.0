namespace RoadExp.Measurements.Core
{
    /// <summary>
    /// Определяет компонент, который может транслировать ошибки в вызывающий код    
    /// </summary>
    public interface IMeasurementErrorProvider
    {
        /// <summary>
        /// Генерируется при возникновении ошибки в работе компонента
        /// </summary>
        event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
    }
}
