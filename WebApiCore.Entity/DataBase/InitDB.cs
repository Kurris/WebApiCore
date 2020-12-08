using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Utils;

namespace WebApiCore.EF.DataBase
{
    public class InitDB
    {
        public static IDataBaseOperation Create(string provider = "SqlServer", string connStr = null)
        {
            provider = GlobalInvariant.SystemConfig?.DBConfig?.Provider ?? provider;

            switch (provider)
            {
                case "SqlServer":
                    connStr = GlobalInvariant.SystemConfig?.DBConfig?.SqlServerConnectionString ?? connStr;
                    return new SqlServerDB(provider, connStr).GetIDataBaseOperation();
                case "MySql":

                    throw new NotImplementedException("MySql尚未实现");
                default:
                    throw new NotImplementedException("未知的数据引擎");
            }
        }
    }
}
