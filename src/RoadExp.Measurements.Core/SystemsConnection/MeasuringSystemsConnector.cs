namespace RoadExp.Measurements.Core.SystemsConnection
{
    /// <summary>
    /// Соединяет измерительную систему с исполнителем команд
    /// (другой измерительной системой)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MeasuringSystemsConnector<T> : IDisposable where T : IDataItem
    {
        #region private fields
        private readonly IMeasuringSystem<T> _source;
        private readonly ITriggered _target;
        private readonly ITriggerProvider<T> _triggerProvider;
        #endregion

        #region constructor
        /// <summary>
        /// Создает коннектор измерительных систем
        /// </summary>
        /// <param name="source">Измерительная система - источник данных</param>
        /// <param name="target">Измерительная система - исполнитель команд</param>
        /// <param name="triggerProvider">Обработчик данных, поступающих от источника данных</param>        
        public MeasuringSystemsConnector(IMeasuringSystem<T> source, ITriggered target, ITriggerProvider<T> triggerProvider)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _triggerProvider = triggerProvider ?? throw new ArgumentNullException(nameof(triggerProvider));
        }
        #endregion

        #region properties
        public bool IsConnected { get; private set; } = false;
        #endregion

        #region public methods
        public void Connect()
        {
            _source.DataReceived += SourceDataReceived;
            _triggerProvider.Triggered += Trigger;
            IsConnected = true;
        }

        public void Disconnect()
        {
            _source.DataReceived -= SourceDataReceived;
            _triggerProvider.Triggered -= Trigger;
            IsConnected = false;
        }

        public void Dispose()
        {
            if (IsConnected)
            {
                Disconnect();
            }
        }
        #endregion

        #region private methods
        private void SourceDataReceived(object? sender, DataEventArgs<T> e)
        {
            _triggerProvider.Process(e.Data);
        }

        private void Trigger(object? sender, EventArgs e)
        {
            _target.Trigger();
        }
        #endregion
    }
}
