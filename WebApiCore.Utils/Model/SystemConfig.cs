namespace WebApiCore.Utils.Model
{
    public class SystemConfig
    {
        public DBConfig DBConfig { get; set; }


        /// <summary>
        /// Jwt设置
        /// </summary>
        public JwtSetting JwtSetting { get; set; }


        /// <summary>
        /// 缓存引擎
        /// </summary>
        public string CacheProvider { get; set; }

        /// <summary>
        /// 登录引擎
        /// </summary>
        public string LoginProvider { get; set; }


        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }


        /// <summary>
        /// 是否在调试中
        /// </summary>
        public bool? IsDebug { get; set; }


        /// <summary>
        /// 允许访问来源
        /// </summary>
        public string AllowHosts { get; set; }
    }
}
