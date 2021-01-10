using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCore.Data.EF.DataBase
{
    /// <summary>
    /// SqlServer数据库
    /// </summary>
    internal class SqlServerDB : BaseDatabaseImp
    {
        internal SqlServerDB(string provider, string connStr) : base(provider, connStr)
        {
        }

        public override async Task<int> ExecProcAsync(string procName, IDictionary<string, object> keyValues = null)
        {
            await RunSqlAsync($"EXEC {procName}", keyValues);
            return await GetOperationReuslt();
        }
    }
}
