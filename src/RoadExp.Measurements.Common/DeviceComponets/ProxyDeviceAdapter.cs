using RoadExp.Measurements.Core;
using System.Threading.Channels;

namespace RoadExp.Measurements.Common
{

    /// <summary>
    /// Proxy-адаптер создается в случаях отсутствия соединения
    /// с физическим устройством
    /// </summary>
    public class ProxyDeviceAdapter : IDeviceAdapter
    {
        private readonly string _errorMessage;
        private readonly Channel<MeasurementBatch> _channel;

        public ProxyDeviceAdapter(string errorMessage)
        {
            _errorMessage = errorMessage;
            _channel = Channel.CreateBounded<MeasurementBatch>(1024);
        }

        public ChannelReader<MeasurementBatch> DataChannel => _channel.Reader;

        public event EventHandler<MeasurementErrorEventArgs> ErrorOccurred;
                
        public Task InitializeAsync(CancellationToken token)
        {
            ErrorOccurred?.Invoke(this, new MeasurementErrorEventArgs(_errorMessage));

            return Task.CompletedTask;
        }

        public void Deinitialize()
        {
            ErrorOccurred?.Invoke(this, new MeasurementErrorEventArgs(_errorMessage));            
        }

        public void Dispose()
        {
            
        }       
    }
}
