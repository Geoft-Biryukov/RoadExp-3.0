using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Core;
using RoadExp.GeometryMeasuringSystem;
using RoadExp.GpsMeasuringSystem;
using RoadExp.Infrastructure;
using RoadExp.Measurements.Core.SystemsConnection;
using RoadExp.Measurements.Data.GpsMeasuringSystem.Data;
using RoadExp.Measurements.Data.TestTriggeredSystem;
using RoadExp.Measurements.Data.GeometryMeasuringSystem.Data;
using RoadExp.Measurements.Common.IndexProviders;


namespace RoadExp.Presentation
{
    //public class TestWorker : IHostedService
    //{
    //    private readonly ILoggerFactory _loggerFactory;
    //    private readonly ILogger _logger;

    //    public TestWorker(ILoggerFactory loggerFactory)
    //    {
    //        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    //        _logger = loggerFactory.CreateLogger<TestWorker>();
    //    }

    //    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        _logger.LogInformation("Начало работы worker");
    //        ShowMessage("Start");

    //        var systemsManager = new MeasuringSystemsManager();

    //        RegisterSystems(systemsManager, _loggerFactory);

    //        ShowMessage("Systems are registered");

    //        var systems = CreateSystems(systemsManager);

    //        ShowMessage("Systems are created");

    //        var gpsSystem = systems[nameof(GpsSystem)] as GpsSystem;
    //        var geometrySystem = systems[nameof(GeometrySystem)] as GeometrySystem;
    //        var testSystem = systems[nameof(TestSystem)] as TestSystem;
    //        var testTriggered = systems[nameof(TestTriggeredSystem)] as TestTriggeredSystem.TestTriggeredSystem;

    //        ShowMessage("Created concrete systems");

    //        var triggerProvider = new SimpleTriggerProvider(5);

    //        var connector = new MeasuringSystemsConnector<TestDataItem>(testSystem!, testTriggered!, triggerProvider);

    //        connector.Connect();

    //        ShowMessage("Connector is created");

    //        ShowMessage("Start initialization");

    //        var cts = new CancellationTokenSource();

    //        testSystem.DataReceived += TestSystemDataReceived;

    //        var taskInit1 = testSystem.InitializeAsync();
    //        var taskInit2 = testTriggered.InitializeAsync();

    //        ShowMessage("Systems initialization are started");


    //        Task.Run(() => Spin(cts.Token));

    //        Task.WaitAll(taskInit1, taskInit2);

    //        cts.Cancel();

    //        ShowMessage("Systems initialization complete");

    //        testSystem.StartAcquisition();
    //        testTriggered.StartAcquisition();

    //        ShowMessage("Start file writing");

    //        await Task.Run(() => Run(stoppingToken), stoppingToken);

    //        testSystem.StopAcquisition();
    //        testTriggered.StopAcquisition();

    //        testSystem.Deinitialize();
    //        testTriggered.Deinitialize();

    //        connector.Dispose();
    //    }

    //    private static void ShowMessage(string message)
    //    {
    //        Console.WriteLine(message);
    //    }

    //    private static void RegisterSystems(MeasuringSystemsManager systemsManager, ILoggerFactory loggerFactory)
    //    {
    //        var indexProvider = new TimestampIndexProvider();

    //        systemsManager.RegisterSystemFactory(
    //            nameof(TestSystem),
    //            new TestMeasuringSystemFactory(loggerFactory));

    //        systemsManager.RegisterSystemFactory(
    //            nameof(TestTriggeredSystem),
    //            new TestTriggeredFactory(loggerFactory.CreateLogger("TestTriggered")));

    //        var gpsConverter = new GpsDataConverter();
    //        var gpsParser = new GpsDataParser(gpsConverter, loggerFactory.CreateLogger<GpsDataParser>());
    //        var gpsAdapterFactory = new GpsDeviceAdapterFactory(indexProvider, gpsConverter, loggerFactory);

    //        systemsManager.RegisterSystemFactory(
    //            nameof(GpsSystem),
    //            new GpsSystemFactory(gpsAdapterFactory, gpsParser, loggerFactory));

    //        var geometryConverter = new GeometryDataConverter();
    //        var geometryParser = new GeometryDataParser(geometryConverter, loggerFactory.CreateLogger<GeometryDataParser>());
    //        var geometryAdapterFactory = new GpsDeviceAdapterFactory(indexProvider, gpsConverter, loggerFactory);
    //        systemsManager.RegisterSystemFactory(
    //            nameof(GeometrySystem),
    //            new GeometrySystemFactory(new GeometryDeviceAdapterFactory(indexProvider, loggerFactory), geometryParser, loggerFactory));
    //    }

    //    private static Dictionary<string, IMeasuringSystem> CreateSystems(MeasuringSystemsManager systemsManager)
    //    {
    //        var systems = new Dictionary<string, IMeasuringSystem>();

    //        var testSettings = new TestMeasuringSystemSettings { FileName = "C:\\Tmp\\source.dat", Period = 100, Value = 256 };
    //        var testSystem = systemsManager.CreateSystem<TestDataItem, TestMeasuringSystemSettings>(nameof(TestSystem), testSettings);
    //        systems.Add(nameof(TestSystem), testSystem);

    //        var testTriggeredSettings = new TestTriggeredSettings { FileName = "C:\\Tmp\\triggered.dat" };
    //        var testTriggeredSystem = systemsManager.CreateSystem<TestTriggeredDataItem, TestTriggeredSettings>(nameof(TestTriggeredSystem), testTriggeredSettings);
    //        systems.Add(nameof(TestTriggeredSystem), testTriggeredSystem);            
            
    //        var gpsSettings = new GpsSystemSettings { BaudRate = 11600, PortName = "COM3", IsEmulationMode = false };
    //        var gpsSystem = systemsManager.CreateSystem<GpsDataItem, GpsSystemSettings>(nameof(GpsSystem), gpsSettings);
    //        systems.Add(nameof(GpsSystem), gpsSystem);

    //        var geometrySettings = new GeometrySystemSettings { BaudRate = 11600, PortName = "COM1", IsEmulationMode = false };
    //        var geometrySystem = systemsManager.CreateSystem<GeometryDataItem, GeometrySystemSettings>(nameof(GeometrySystem), geometrySettings);
    //        systems.Add(nameof(GeometrySystem), geometrySystem);

    //        return systems;
    //    }

    //    private static void Spin(CancellationToken token)
    //    {
    //        string[] frames = { "\\", "|", "/", "-" };
    //        int currentFrame = 0;
    //        Console.CursorVisible = false;
    //        while (!token.IsCancellationRequested)
    //        {
    //            Thread.Sleep(50); // Animation speed
    //            Console.Write(frames[currentFrame]);
    //            Thread.Sleep(50); // Animation speed

    //            // Move back one space to overwrite the character on the next loop
    //            Console.Write("\b");

    //            currentFrame = (currentFrame + 1) % frames.Length;
    //        }
    //        Console.CursorVisible = true;
    //    }

    //    private static void TestSystemDataReceived(object? sender, DataEventArgs<TestDataItem> e)
    //    {
    //        var value = e.Data;

    //        if (value == null)
    //            return;

    //        if (e.Data.Value % 10 != 0)
    //            return;

    //        var time = new DateTime(value.Index);

    //        string text = $"Данные получены: {time:HH:mm:ss}, данные: {value.Value}  [Esc для завершения]";

    //        Console.WriteLine(text);
    //    }

    //    private static void Run(CancellationToken stoppingToken)
    //    {
    //        while (true)
    //        {
    //            if (stoppingToken.IsCancellationRequested)
    //                return;

    //            var key = Console.ReadKey();
    //            if (key.Key == ConsoleKey.Escape)
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    public Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        return ExecuteAsync(cancellationToken);
    //    }

    //    public Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    private class SimpleTriggerProvider : ITriggerProvider<TestDataItem>
    //    {
    //        private readonly int _factor;

    //        public SimpleTriggerProvider(int factor)
    //        {
    //            _factor = factor;
    //        }

    //        public event EventHandler<EventArgs> Triggered;

    //        public void Process(TestDataItem item)
    //        {
    //            if (item.Value % _factor == 0)
    //                Triggered.Invoke(this, EventArgs.Empty);
    //        }
    //    }
    //}
}
