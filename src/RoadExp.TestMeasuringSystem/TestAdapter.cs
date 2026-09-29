using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.TestMeasuringSystem;

namespace TestMeasuringSystem
{
    /// <summary>
    /// Тестовый адаптер
    /// Через заданное количество времени генерирует событие
    /// с заданным числом в качестве аргумента, преобразованным 
    /// в массив байтов
    /// </summary>
    public class TestAdapter : IDeviceAdapter<TestDataItem>
    {
        #region private fields
        private readonly object _sync = new object();
        private readonly System.Timers.Timer _timer;
        private readonly long _value;
        private bool _isStarted = false;
        private long _counter = 0;
        #endregion
        public TestAdapter(int periodMs, long value)
        {
            if (periodMs <= 0)
                throw new ArgumentOutOfRangeException(nameof(periodMs));

            _timer = new System.Timers.Timer(periodMs);
            _value = value;
        }

        private void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_isStarted)
                return;

            lock (_sync)
            {
                long index = DateTime.UtcNow.Ticks;
                var data = new TestDataItem(index, _counter++);
                var args = new DataEventArgs<TestDataItem>(data);
                DataReceived?.Invoke(this, args);
            }
        }

        
        public event EventHandler<DataEventArgs<TestDataItem>> DataReceived;
        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;

        private void Initialize()
        {
            Thread.Sleep(5000);
            _counter = 0;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            _isStarted = true;            
        }
        
        public async Task InitializeAsync(CancellationToken token)
        {
            await Task.Run(() => Initialize(), token);
        }

        public void Deinitialize()
        {
            _timer.Stop();
            _timer.Elapsed -= TimerElapsed;
            _isStarted = false;
        }       

        public void Dispose()
        {
            if(_timer != null && _timer.Enabled)
            {
                Deinitialize();
            }

            _timer?.Dispose();
        }

       
    }
}
