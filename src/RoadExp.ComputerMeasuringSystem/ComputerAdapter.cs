using Microsoft.Extensions.Logging;
using RoadExp.ComputerMeasuringSystem.Data;
using RoadExp.Measurements.Core;


namespace RoadExp.ComputerMeasuringSystem
{
    internal class ComputerAdapter : IDeviceAdapter<ComputerDataItem>
    {
        #region private fields
        private readonly int _elementsCount;
        private readonly ILogger<ComputerAdapter> _logger;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Task _worker;

        private readonly ComputerInfoHeader _header;
        #endregion

        public ComputerAdapter(int elementsCount, ILogger<ComputerAdapter> logger)
        {
            _elementsCount = elementsCount;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _header = CreateHeader();
            _worker = new Task(() => Process(_cts.Token), _cts.Token);
        }

        public event EventHandler<DataEventArgs<ComputerDataItem>> DataReceived;
        public event EventHandler<Measurements.Core.MeasurementErrorEventArgs> ErrorOccurred;

        public async Task InitializeAsync(CancellationToken token)
        {
            _logger.LogInformation($"Запущена инициализация {nameof(ComputerAdapter)}");

            await Task.Run(() => Initialize(token), token);

            _logger.LogInformation($"Инициализация {nameof(ComputerAdapter)} успешно завершена");

            _worker.Start();
        }

        public void Deinitialize()
        {
            _logger.LogInformation($"Запущена деинициализация {nameof(ComputerAdapter)})");

            if (!_worker.IsCanceled)
                _cts.Cancel();

            _worker.Wait();
        }

        private void Initialize(CancellationToken token)
        {
            int cpuUsage = ManagementHelpers.GetCpuUsage();
            ulong ram = ManagementHelpers.GetAvailableRam();           
        }

        private void Process(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                long index = Environment.TickCount64;

                int cpuUsage = ManagementHelpers.GetCpuUsage();
                ulong ram = ManagementHelpers.GetAvailableRam();
                
                var data = new ComputerPerformanceInfo { CpuUsage = cpuUsage, AvailableRam = ram };
                var item = new ComputerDataItem(index, _header, data);

                Thread.Sleep(100);

                DataReceived?.Invoke(this, new DataEventArgs<ComputerDataItem>(item));
            }
        }

        private ComputerInfoHeader CreateHeader()
            => new ComputerInfoHeader
            {
                MachineName = Environment.MachineName,
                OsVersion = Environment.OSVersion.VersionString,
                ProcessorCount = Environment.ProcessorCount,
                UserDomain = Environment.UserDomainName,
                UserName = Environment.UserName,
            };

        public void Dispose()
        {
            Deinitialize();
            _cts?.Dispose();
        }
    }
}
