using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    /// <summary>
    /// MySql数据库
    /// </summary>
    internal class MySqlDB : BaseDatabase
    {
        internal MySqlDB(string provider, string connStr) : base(provider, connStr)
        {
        }

        /*----------------------------------------------重写基类默认的sql行为---------------------------------------------------*/

        public override async Task<int> ExecProcAsync(string procName, IDictionary<string, object> keyValues = null)
        {
            await RunSqlAsync($"CALL {procName};", keyValues);
            return await GetReuslt();
        }
    }
}
