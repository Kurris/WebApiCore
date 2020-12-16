namespace WebApiCore.Utils.Model
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtSetting
    {
        public string TokenName { get; set; }
        public string TokenKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public int Expiration { get; set; }
    }
}
