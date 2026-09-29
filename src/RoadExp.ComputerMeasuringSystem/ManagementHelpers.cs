using System.Management;

namespace RoadExp.ComputerMeasuringSystem
{
    internal static class ManagementHelpers
    {
        public static int GetCpuUsage()
        {
            // Query the real-time formatted performance data for processors
            using var searcher = new ManagementObjectSearcher(
                "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'");

            foreach (ManagementObject obj in searcher.Get())
            {
                return Convert.ToInt32(obj["PercentProcessorTime"]);
            }
            return 0;
        }

        public static ulong GetAvailableRam()
        {
            // Query the physical memory configuration status
            using var searcher = new ManagementObjectSearcher(
                "SELECT FreePhysicalMemory FROM Win32_OperatingSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                // FreePhysicalMemory is in Kilobytes, convert to Megabytes
                return (ulong)obj["FreePhysicalMemory"] / 1024;
            }
            return 0;
        }
    }
}
