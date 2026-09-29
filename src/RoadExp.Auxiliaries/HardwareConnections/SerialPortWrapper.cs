using System.IO.Ports;

namespace RoadExp.Auxiliaries.HardwareConnections
{
    /// <summary>
    /// Обёртка над SerialPort
    /// </summary>
    public class SerialPortWrapper : IHardwareConnector
    {
        #region private fields
        private readonly SerialPort _port;
        #endregion

        #region constructor
        public SerialPortWrapper(SerialPort port)
        {
            _port = port ?? throw new ArgumentNullException(nameof(port));            
        }        
       
        #endregion

        #region events
        public event SerialDataReceivedEventHandler DataReceived
        {
            add => _port.DataReceived += value;
            remove => _port.DataReceived -= value;
        }       

        public event SerialErrorReceivedEventHandler ErrorReceived
        {
            add => _port.ErrorReceived += value;
            remove => _port.ErrorReceived -= value;

        }       
        #endregion

        #region properties
        public string PortName
        {
            get => _port.PortName;
            set => _port.PortName = value;
        }

        public int BaudRate
        { 
            get => _port.BaudRate; 
            set => _port.BaudRate = value; 
        }               

        public Stream BaseStream
        {
            get => _port.BaseStream;            
        }

        public bool IsOpen
        {
            get => _port.IsOpen;
        }        

        #endregion

        #region public methods

        public void Open() => _port.Open();

        public void Close() => _port.Close();            

        public void Dispose()
        {
            _port.Dispose();
        }
        #endregion
    }
}
