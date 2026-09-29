using System.IO.Ports;

namespace RoadExp.Auxiliaries.HardwareConnections
{
    public interface IHardwareConnector : IDisposable
    {       
        Stream BaseStream { get; }
       
        event SerialDataReceivedEventHandler DataReceived;       
        event SerialErrorReceivedEventHandler ErrorReceived;
               
        void Open();
        void Close();
        
        bool IsOpen { get; }        
    }
}
