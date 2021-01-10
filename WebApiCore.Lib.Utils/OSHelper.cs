using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Lib.Utils
{
    /// <summary>
    /// OS帮助类
    /// </summary>
    public class OSHelper
    {
        /// <summary>
        /// 判断是否为Unix
        /// </summary>
        public static bool IsUnix { get => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux); }

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public static async Task<OSInfo> GetOSInfo()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            OSInfo computerInfo = new OSInfo();
            try
            {
                MemoryMetricsHelper helper = new MemoryMetricsHelper();
                MemoryMetrics memoryMetrics = helper.GetMetrics();
                computerInfo.TotalRAM = Math.Ceiling(memoryMetrics.Total / 1024).ToString() + " GB";
                computerInfo.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + " %";
                computerInfo.CPURate = Math.Ceiling(Convert.ToDouble(GetCPURate())) + " %";
                computerInfo.RunTime = GetOSRunTime();

                return computerInfo;
            }
            catch
            {
                return null;
            }
        }


        public static string GetCPURate()
        {
            string cpuRate = string.Empty;
            if (IsUnix)
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                string output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }
            return cpuRate;
        }

        /// <summary>
        /// 获取OX运行时间
        /// </summary>
        /// <returns></returns>
        public static string GetOSRunTime()
        {
            string runTime = string.Empty;
            try
            {
                if (IsUnix)
                {
                    string output = ShellHelper.Bash("uptime -s");
                    output = output.Trim();
                    runTime = DateTimeHelper.UnixTimeToString((DateTime.Now - output.ParseToDateTime())
                                            .TotalMilliseconds.ParseToStr().Split('.').First().ParseToLong());
                }
                else
                {
                    string output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                    string[] outputArr = output.Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (outputArr.Length == 2)
                    {
                        runTime = DateTimeHelper.UnixTimeToString((DateTime.Now - outputArr[1].Split('.')[0].ParseToDateTime())
                                                .TotalMilliseconds.ParseToStr().Split('.')[0].ParseToLong());
                    }
                }

                return runTime;
            }
            catch
            {
                return null;
            }
        }
    }





    internal class MemoryMetricsHelper
    {
        public MemoryMetrics GetMetrics()
        {
            if (OSHelper.IsUnix)
            {
                return GetUnixMetrics();
            }
            return GetWindowsMetrics();
        }
        private MemoryMetrics GetWindowsMetrics()
        {
            string output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }
        private MemoryMetrics GetUnixMetrics()
        {
            string output = ShellHelper.Bash("free -m");

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);

            return metrics;
        }
    }

    /// <summary>
    /// 内存指标
    /// </summary>
    internal class MemoryMetrics
    {
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
    }

    /// <summary>
    /// OS信息
    /// </summary>
    public class OSInfo
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPURate { get; set; }
        /// <summary>
        /// 总内存
        /// </summary>
        public string TotalRAM { get; set; }
        /// <summary>
        /// 内存使用率
        /// </summary>
        public string RAMRate { get; set; }
        /// <summary>
        /// 系统运行时间
        /// </summary>
        public string RunTime { get; set; }
    }
}
