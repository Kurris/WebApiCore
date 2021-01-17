using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebApiCore.Lib.Model
{
    /// <summary>
    /// OS信息
    /// </summary>
    public class OSMetrics
    {
        public string MachineName { get => Environment.MachineName; }
        public string Platform { get => Environment.OSVersion.Platform.ToString(); }
        public string Version { get => Environment.OSVersion.VersionString; }
        public bool IsUnix { get => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux); }
        public bool Is64 { get => Environment.Is64BitOperatingSystem; }
        public string RunTime { get; set; }
        public CpuMetrics Cpu { get; set; }
        public MemoryMetrics Memory { get; set; }
    }


    /// <summary>
    /// CPU
    /// </summary>
    public class CpuMetrics
    {
        /// <summary>
        /// Cpu名称
        /// </summary>
        public string CpuName { get; set; }

        /// <summary>
        /// 核心数
        /// </summary>
        public int CoreCount { get; set; }

        /// <summary>
        /// 线程数
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        /// 使用量
        /// </summary>
        public string Used { get; set; }
    }

    /// <summary>
    /// 内存
    /// </summary>
    public class MemoryMetrics
    {
        /// <summary>
        /// 总量
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// 使用量
        /// </summary>
        public string Used { get; set; }

        /// <summary>
        /// 空闲
        /// </summary>
        public string Free { get; set; }
    }
}
