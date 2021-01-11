using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Lib.Utils.Config
{
    /// <summary>
    /// 系统配置信息
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public DBConfig DBConfig { get; set; }


        /// <summary>
        /// Jwt设置
        /// </summary>
        public JwtConfig JwtConfig { get; set; }


        /// <summary>
        /// 缓存引擎
        /// </summary>
        public string CacheProvider { get; set; }

        /// <summary>
        /// 登录引擎
        /// <code>
        /// Jwt,Cookie,Session
        /// </code>
        /// </summary>
        public string LoginProvider { get; set; }


        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }


        /// <summary>
        /// 是否在调试中
        /// </summary>
        public bool IsDebug { get; set; }


        /// <summary>
        /// 允许访问来源
        /// </summary>
        public string AllowHosts { get; set; }
    }
}
