using Microsoft.Extensions.Logging;
using RoadExp.Measurements.Common;
using RoadExp.Measurements.Core;
using RoadExp.Measurements.Core.Serialization;

namespace RoadExp.Presentation
{
    /// <summary>
    /// Тестовый класс для проверки 
    /// корректности работы DataStorage
    /// </summary>
    //internal class TestingDataStorage
    //{        
    //    private readonly IDataStorage<SimplestDataItem> _storage;
    //    private readonly ILogger _logger;

    //    private TestingDataStorage(IDataStorage<SimplestDataItem> storage, ILogger logger)
    //    {
    //        _storage = storage;
    //        _logger = logger;
    //    }

    //    public static TestingDataStorage Create(int delay, ILogger logger)
    //    {
    //        //var stream = new MemoryStream();

    //        //var streamWriter = new StreamWriter(stream);

    //        //var writer = new SimplestDataWriter(streamWriter, delay);
    //        var writer = new ConsoleDataWriter(delay);

    //        var storage = new DataStorage<SimplestDataItem>(10, writer, logger);

    //        return new TestingDataStorage(storage, logger);
    //    }

    //    public async Task RunAsync(CancellationToken token)
    //    {
    //        await Task.Run(() => Run(token), token);
    //    }

    //    public void Run(CancellationToken token)
    //    {
    //        int counter = 0;
    //        _storage.StartSaving();
    //        while (true)
    //        {
    //            if (token.IsCancellationRequested)
    //                break;

    //            long index = DateTime.UtcNow.Ticks;

    //            var data = new SimplestDataItem(index, counter++);

    //            _storage.Add(data);

    //            Thread.Sleep(1);

    //        }
    //        _storage.StopSaving();
    //    }

    //    #region Simplest
    //    public class SimplestDataItem : IDataItem<int>
    //    {
    //        public SimplestDataItem(long index, int value)
    //        {
    //            Index = index;
    //            Value = value;
    //        }

    //        public long Index { get; }

    //        public int Value { get; }
    //    }

    //    private class SimplestDataItemSerializer
    //    {
    //        public string Serialize(SimplestDataItem item)
    //        {
    //            var dt = new DateTime(item.Index);
    //            return $"Время: {dt:hh:mm:ss}; Значение: {item.Value}";
    //        }
    //    }

    //    public class SimplestDataWriter : IDataWriter<SimplestDataItem>
    //    {
    //        private readonly StreamWriter _innerWriter;
    //        private readonly int _delay;
    //        private readonly SimplestDataItemSerializer _serializer = new SimplestDataItemSerializer();

    //        public SimplestDataWriter(StreamWriter writer, int delay = 0)
    //        {
    //            _innerWriter = writer;
    //            _delay = delay;
    //        }

    //        public void Dispose()
    //        {
    //            _innerWriter.Dispose();
    //        }

    //        public void Flush()
    //        {
    //            _innerWriter?.Flush();
    //        }

    //        public void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public void Write(SimplestDataItem value)
    //        {
    //            var str = _serializer.Serialize(value);
    //            _innerWriter.WriteLine(str);

    //            if(_delay > 0)
    //                Thread.Sleep(_delay);
    //        }
    //    }

    //    public class ConsoleDataWriter : IDataWriter<SimplestDataItem>
    //    {            
    //        private readonly int _delay;
    //        private readonly SimplestDataItemSerializer _serializer = new SimplestDataItemSerializer();

    //        public ConsoleDataWriter(int delay = 0)
    //        {                
    //            _delay = delay;
    //        }

    //        public void Dispose()
    //        {
                
    //        }

    //        public void Flush()
    //        {
                
    //        }

    //        public void OpenFile(string fileName, CommonFileHeader fileHeader, byte[] metadata)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public void Write(SimplestDataItem value)
    //        {
    //            var str = _serializer.Serialize(value);
    //            System.Diagnostics.Debug.WriteLine(str);

    //            if (_delay > 0)
    //                Thread.Sleep(_delay);
    //        }
    //    }
    //    #endregion
    //}
}
