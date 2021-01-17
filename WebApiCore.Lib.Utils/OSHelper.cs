using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Lib.Model;

namespace WebApiCore.Lib.Utils
{
    /// <summary>
    /// OS帮助类
    /// </summary>
    public class OSHelper
    {
        private static readonly OSMetrics _osMetrics = new OSMetrics();

        public static OSMetrics GetOSMetrics()
        {
            try
            {
                _osMetrics.Memory = GetMemoryMetrics();
                _osMetrics.Cpu = GetCpuMetrics();
                _osMetrics.RunTime = DateTimeHelper.GetDateTime(Environment.TickCount64);

                //return new OSInfo()
                //{
                //    TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024).ToString() + " GB",
                //    RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + " %",
                //    CPURate = GetCPURate(),
                //    RunTime = GetRunTime(),
                //};
                return _osMetrics;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取CPU使用率
        /// </summary>
        /// <returns></returns>
        private static string GetCPURate()
        {
            string cpuRate;
            if (_osMetrics.IsUnix)
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                string output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }

            return Math.Ceiling(Convert.ToDouble(cpuRate)) + " %";
        }

        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        private static MemoryMetrics GetMemoryMetrics()
        {
            if (_osMetrics.IsUnix)
            {
                return GetUnixMetrics();
            }
            return GetWindowsMetrics();

            MemoryMetrics GetWindowsMetrics()
            {
                string output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

                var lines = output.Trim().Split("\n");
                var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
                var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

                double total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
                double free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);

                var metrics = new MemoryMetrics
                {
                    Total = Math.Ceiling(total / 1024) + " GB",
                    Free = Math.Ceiling(free / 1024) + " GB",
                    Used = Math.Round((total - free) / total * 100,0) + " %"
                };

                return metrics;
            }

            MemoryMetrics GetUnixMetrics()
            {
                string output = ShellHelper.Bash("free -m");

                var lines = output.Split("\n");
                var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                double total = double.Parse(memory[1]);
                double used = double.Parse(memory[2]);

                var metrics = new MemoryMetrics
                {
                    Total = Math.Ceiling(total / 1024) + " GB",
                    Used = Math.Ceiling(used / 1024) + " GB",
                    Free = (total - used) / 1024 + " GB"
                };

                return metrics;
            }
        }

        private static CpuMetrics GetCpuMetrics()
        {
            var cpu = new CpuMetrics();
            string cpuRate;
            if (_osMetrics.IsUnix)
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                cpu.CpuName = ShellHelper.Cmd("wmic", "cpu get Name").Replace("Name", string.Empty).Trim();
                cpu.CoreCount = Convert.ToInt32(ShellHelper.Cmd("wmic", "cpu get NumberOfCores").Replace("NumberOfCores", string.Empty).Trim());
                cpu.ThreadCount = Convert.ToInt32(ShellHelper.Cmd("wmic", "cpu get ThreadCount").Replace("ThreadCount", string.Empty).Trim());
                cpu.Used = ShellHelper.Cmd("wmic", "cpu get LoadPercentage").Replace("LoadPercentage", string.Empty).Trim() + " %";
            }

            return cpu;
        }
    }
}
