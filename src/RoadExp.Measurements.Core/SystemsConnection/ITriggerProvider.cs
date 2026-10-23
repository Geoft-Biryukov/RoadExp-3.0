namespace RoadExp.Measurements.Core.SystemsConnection
{
    public interface ITriggerProvider
    {
        /// <summary>
        /// Событие, генерируемое по наступлению
        /// определенных условий
        /// </summary>
        event EventHandler<EventArgs> Triggered;
    }

    /// <summary>
    /// Генерирует событие по наступлении 
    /// определенных условий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITriggerProvider<in T> : ITriggerProvider where T : IDataItem
    {
        /// <summary>
        /// Обрабатывает данные от измерительной системы
        /// </summary>
        /// <param name="item"></param>
        void Process(T item);       
    }
}
