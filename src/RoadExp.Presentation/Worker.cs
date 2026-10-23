using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoadExp.GeometryMeasuringSystem;
using RoadExp.GeometryMeasuringSystem.MeasuringSystem;
using RoadExp.GpsMeasuringSystem;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;

namespace RoadExp.Presentation
{
    public class Worker : IHostedService
    {
        private readonly IEnumerable<IMeasuringSystemManager> _managers;
        private readonly ILogger<Worker> _logger;

        public Worker(IEnumerable<IMeasuringSystemManager> managers, ILoggerFactory loggerFactory)
        {
            _managers = managers ?? throw new ArgumentNullException(nameof(managers));
            _logger = loggerFactory.CreateLogger<Worker>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Worker запущен");

            UpdateSettings();

            SubscribeDataReceived();

            InitializeAll();

            _logger.LogDebug("Инициализация всех систем завершена");

            StartAll();

            Run(cancellationToken);

            DeinitializeAll();

            _logger.LogDebug("Деинициализация всех систем завершена");            

            UnsubscribeOnDataReceived();

            foreach (var manager in _managers)
            {
                manager.Dispose();
            }
            return Task.CompletedTask;
        }

        private void UpdateSettings()
        {
            foreach (var manager in _managers)
            {
                manager.Settings.Path = @"C:\Tmp\RoadExp\Console";
            }
        }

        private void SubscribeDataReceived()
        {
            var m = _managers.FirstOrDefault(item => item is GpsSystemManager);

            if (m != null && m.MeasuringSystem is GpsSystem gpsSystem)
            {
                _logger.LogDebug("Подписка на обновление данных gps");
                gpsSystem.DataReceived += GpsDataReceived;
            }

            m = _managers.FirstOrDefault(item => item is GeometrySystemManager);

            if (m != null && m.MeasuringSystem is GeometrySystem geometrySystem)
            {
                _logger.LogDebug("Подписка на обновление данных геометрии");
                geometrySystem.DataReceived += GeometrySystemDataReceived;
            }
        }

        private void UnsubscribeOnDataReceived()
        {
            var m = _managers.FirstOrDefault(item => item is GpsSystemManager);

            if (m != null && m.MeasuringSystem is GpsSystem gpsSystem)
            {
                _logger.LogDebug("Отписка от обновления данных gps");
                gpsSystem.DataReceived -= GpsDataReceived;
            }

            m = _managers.FirstOrDefault(item => item is GeometrySystemManager);

            if (m != null && m.MeasuringSystem is GeometrySystem geometrySystem)
            {
                _logger.LogDebug("Отписка от обновления данных геометрии");
                geometrySystem.DataReceived -= GeometrySystemDataReceived;
            }
        }

        private void GeometrySystemDataReceived(object? sender, DataEventArgs<GeometryDataItem> e)
        {
            long index = e.Data.Index;
            var data = e.Data.Value;
            var message = $"Индекс минс: {index} | Широта: {data.Latitude:N5} | Долгота: {data.Longitude:N5}";
            Console.WriteLine(message);
        }

        private void GpsDataReceived(object? sender, DataEventArgs<GpsDataItem> e)
        {
            var gga = e.Data.Value.Gga;
            var message = $"Время gps: {gga.UtcTime:HH:mm:ss} | Широта: {gga.Latitude:N5} | Долгота: {gga.Longitude:N5}";
            Console.WriteLine(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void InitializeAll()
        {
            var tasks = new List<Task>();

            foreach (var manager in _managers)
            {
                tasks.Add(manager.MeasuringSystem.InitializeAsync());
            }

            Task.WaitAll(tasks.ToArray());            
        }
       
        private void DeinitializeAll()
        {
            foreach (var manager in _managers)
            {
                manager.MeasuringSystem.Deinitialize();
            }
        }
        
        private void StartAll()
        {
            _logger.LogInformation("Запускается сохранение в файл для всех систем");

            var tasks = new List<Task>();

            foreach (var manager in _managers)
            {                               
                manager.MeasuringSystem.StartAcquisition();
            }            
        }

        private void StopAll()
        {
            _logger.LogInformation("Остановка сохранения в файл для всех систем");

            var tasks = new List<Task>();

            foreach (var manager in _managers)
            {
                manager.MeasuringSystem.StopAcquisition();
            }
        }

        private void Run(CancellationToken stoppingToken)
        {
            while (true)
            {
                if (stoppingToken.IsCancellationRequested)
                    return;

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    _logger.LogDebug("Нажат esc");
                    break;
                }
            }
        }
    }
}
