namespace RoadExp.Measurements.Core.SystemsConnection
{
    /// <summary>
    /// Генерирует событие по наступлении 
    /// определенных условий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITriggerProvider<T> where T : IDataItem
    {
        /// <summary>
        /// Обрабатывает данные от измерительной системы
        /// </summary>
        /// <param name="item"></param>
        void Process(T item);

        /// <summary>
        /// Событие, генерируемое по наступлению
        /// определенных условий
        /// </summary>
        event EventHandler<EventArgs> Triggered;
    }
}
