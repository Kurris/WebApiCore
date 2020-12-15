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
    }
}
