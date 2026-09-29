using Serilog.Core;
using Serilog.Events;

namespace RoadExp.Auxiliaries.Logger
{
    public class RoadExpConsoleSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        public RoadExpConsoleSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public static event Action<string>? OnLogReceived;

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);

            OnLogReceived?.Invoke(message);
        }
    }
}
