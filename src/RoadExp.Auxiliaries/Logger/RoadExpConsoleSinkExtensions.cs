using Serilog;
using Serilog.Configuration;

namespace RoadExp.Auxiliaries.Logger
{
    public static class RoadExpConsoleSinkExtensions
    {
        public static LoggerConfiguration RoadExpConsole(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        IFormatProvider? formatProvider = null)
        {
            return loggerSinkConfiguration.Sink(new RoadExpConsoleSink(formatProvider));
        }
    }
}
