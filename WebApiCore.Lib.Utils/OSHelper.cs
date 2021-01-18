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

        /// <summary>
        /// 获取OS信息
        /// </summary>
        /// <returns></returns>
        public static OSMetrics GetOSMetrics()
        {
            try
            {
                _osMetrics.Memory = GetMemoryMetrics();
                _osMetrics.Cpu = GetCpuMetrics();
                _osMetrics.RunTime = DateTimeHelper.GetDateTime(Environment.TickCount64);

                return _osMetrics;
            }
            catch
            {
                return null;
            }
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
                    Used = Math.Round((total - free) / total * 100, 0) + " %"
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
                    Used = Math.Round((used / total) * 100, 0) + " %",
                    Free = Math.Ceiling((total - used) / 1024) + " GB"
                };

                return metrics;
            }
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns></returns>
        private static CpuMetrics GetCpuMetrics()
        {
            var cpu = new CpuMetrics();

            if (_osMetrics.IsUnix)
            {
                var cpuName = ShellHelper.Bash("cat /proc/cpuinfo | grep name | cut -f2 -d: | uniq -c").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                cpu.CoreCount = Convert.ToInt32(cpuName[0].Trim());
                cpuName[0] = string.Empty;
                cpu.CpuName = string.Join(" ", cpuName).TrimStart().Trim();
                cpu.ThreadCount = Convert.ToInt32(ShellHelper.Bash("grep 'processor' /proc/cpuinfo | sort -u | wc -l").Trim());
                cpu.Used = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'").Trim() + " %";
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
