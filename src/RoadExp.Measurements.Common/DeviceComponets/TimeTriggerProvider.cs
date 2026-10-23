using RoadExp.Measurements.Core.SystemsConnection;

namespace RoadExp.Measurements.Common
{
    /// <summary>
    /// Генерирует сигнал по времени
    /// </summary>
    public class TimeTriggerProvider : ITriggerProvider, IDisposable
    {        
        private readonly Timer _timer;

        /// <summary>
        /// Создает объект TimeTriggerProvider
        /// </summary>
        /// <param name="triggerTime">Время в миллисекундах для генерации сигнала</param>
        public TimeTriggerProvider(int triggerTime = 1000)
        {     
            _timer = new Timer(TimerTick, null, 0, triggerTime);            
        }

        public event EventHandler<EventArgs> Triggered;

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void TimerTick(object? obj)
        {
            Triggered?.Invoke(this, EventArgs.Empty);
        }
    }
}
