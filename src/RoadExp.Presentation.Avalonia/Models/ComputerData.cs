namespace RoadExp.Presentation.Avalonia.Models
{
    public class ComputerData
    {
        public ComputerData(int processorUsage, ulong availableMemory)
        {
            ProcessorUsage = processorUsage;
            AvailableMemory = availableMemory;
        }

        public int ProcessorUsage { get; }
        public ulong AvailableMemory { get; }
    }
}
