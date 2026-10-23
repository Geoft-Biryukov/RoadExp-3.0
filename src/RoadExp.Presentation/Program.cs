// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoadExp.Auxiliaries.Logger;
using RoadExp.Infrastructure;
using RoadExp.Presentation;
using Serilog;
using Serilog.Filters;

internal class Program
{
    private static void Main(string[] args)
    {
        var settings = new HostApplicationBuilderSettings
        {
            Args = args,
            ApplicationName = "RoadExp 3 Test version",
            ContentRootPath = Directory.GetCurrentDirectory()
        };
                
        var builder = Host.CreateApplicationBuilder(settings);

        ConfigureSerilog();

        //Создаем стандартную фабрику логов и связываем её с Serilog
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            // Метод расширения AddSerilog связывает Microsoft-логирование с Serilog
            builder.AddSerilog(dispose: true);
        });

        builder.Services.AddSerilog();

        builder.Services.AddMeasuringSystemManagers(loggerFactory);
       
        builder.Services.AddHostedService<Worker>();

        using var host = builder.Build();                      

        host.Run();                 
    }

    private static void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/app-debug.txt",
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                                rollingInterval: RollingInterval.Day)
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToUser"))
                .WriteTo.RoadExpConsole())
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToGpsSystem"))
                .WriteTo.File("logs/gps-system.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToGeometrySystem"))
                .WriteTo.File("logs/geometry-system.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(Matching.WithProperty("ToTestSystem"))
                .WriteTo.File("logs/test-system.txt", rollingInterval: RollingInterval.Day))
            .WriteTo.Console()
            .CreateLogger();
    }         
}