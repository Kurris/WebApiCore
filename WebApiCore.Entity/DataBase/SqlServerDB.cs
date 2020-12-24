using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    /// <summary>
    /// SqlServer数据库
    /// </summary>
    internal class SqlServerDB : BaseDatabase
    {
        internal SqlServerDB(string provider, string connStr) : base(provider, connStr)
        {
        }

        public override async Task<int> ExecProcAsync(string procName, IDictionary<string, object> keyValues = null)
        {
            await RunSqlAsync($"EXEC {procName}", keyValues);
            return await GetReuslt();
        }
    }
}
