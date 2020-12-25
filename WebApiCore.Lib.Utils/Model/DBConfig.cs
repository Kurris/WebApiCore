namespace WebApiCore.Lib.Utils.Model
{
    public class DBConfig
    {
        public string Provider { get; set; }

        public string SqlServerConnectionString { get; set; }

        public string MySqlConnectionString { get; set; }

        public int Timeout { get; set; }

        public int DBSlowSqlLogTime { get; set; }
    }
}
