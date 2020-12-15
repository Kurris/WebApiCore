using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using WebApiCore.Utils.Extensions;
using WebApiCore.Utils.Model;

namespace WebApiCore.Utils
{
    /// <summary>
    /// 全局类
    /// </summary>
    public class GlobalInvariant
    {
        /// <summary>
        /// 系统参数配置
        /// </summary>
        public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// Service引擎
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置管理
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        private string _version = string.Empty;
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get
            {
                if (_version.IsEmpty())
                {
                    Version version = Assembly.GetEntryAssembly().GetName().Version;
                    _version = version.Major + "." + version.Minor;
                }
                return _version;
            }
        }
    }
}
