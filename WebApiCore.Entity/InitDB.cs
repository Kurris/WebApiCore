using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.EF.DataBase;
using WebApiCore.Utils;

namespace WebApiCore.EF
{
    /// <summary>
    /// 初始化数据库上下文
    /// </summary>
    public class InitDB
    {
        /// <summary>
        /// 创建具体数据库
        /// </summary>
        /// <param name="provider">数据库引擎</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns>操作对象<see cref="IDataBaseOperation"/></returns>
        public static IDataBaseOperation Create(string provider = null, string connStr = null)
        {
            provider = GlobalInvariant.SystemConfig?.DBConfig?.Provider ?? provider;

            switch (provider)
            {
                case "SqlServer":
                    connStr = GlobalInvariant.SystemConfig?.DBConfig?.SqlServerConnectionString ?? connStr;
                    return new SqlServerDB(provider, connStr).GetIDataBaseOperation();
                case "MySql":
                    connStr = GlobalInvariant.SystemConfig?.DBConfig?.MySqlConnectionString ?? connStr;
                    return new MySqlDB(provider, connStr).GetIDataBaseOperation();


                default:
                    throw new NotImplementedException("未知的数据引擎");
            }
        }
    }
}
