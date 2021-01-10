namespace WebApiCore.Lib.Utils.Model
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConfig
    {
        /// <summary>
        /// 数据库引擎
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Sqlserver 连接字符串
        /// </summary>
        public string SqlServerConnectionString { get; set; }

        /// <summary>
        /// MySql连接字符串
        /// </summary>
        public string MySqlConnectionString { get; set; }

        /// <summary>
        /// 超时时间(sec)
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 慢SQL判定时间(sec)
        /// </summary>
        public int DBSlowSqlLogTime { get; set; }
    }
}
