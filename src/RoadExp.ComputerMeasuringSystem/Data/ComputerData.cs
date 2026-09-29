namespace RoadExp.ComputerMeasuringSystem.Data
{
    public class ComputerData
    {
        public ComputerData(ComputerInfoHeader header, ComputerPerformanceInfo performance)
        {
            Header = header;
            Performance = performance;
        }

        public ComputerInfoHeader Header { get; }
        public ComputerPerformanceInfo Performance { get; }
    }
}
