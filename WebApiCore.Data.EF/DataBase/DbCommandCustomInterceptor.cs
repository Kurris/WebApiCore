using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Data.EF.DataBase
{
    /// <summary>
    /// 自定义数据库拦截器
    /// </summary>
    internal class DbCommandCustomInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<DbCommandCustomInterceptor> _logger = GlobalInvariant.ServiceProvider?.GetService<ILogger<DbCommandCustomInterceptor>>();

        public override async Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalInvariant.SystemConfig?.DBConfig.DBSlowSqlLogTime * 1000)
            {
                string log = $"耗时:{eventData.Duration.TotalSeconds}秒\r\n" +
                             $"语句:{command.CommandText}";
                _logger?.LogWarning(log);
            }
            return await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override async Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalInvariant.SystemConfig?.DBConfig.DBSlowSqlLogTime * 1000)
            {
                string log = $"耗时:{eventData.Duration.TotalSeconds}秒\r\n" +
                             $"语句:{command.CommandText}";
                _logger?.LogWarning(log);
            }
            return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override async Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalInvariant.SystemConfig?.DBConfig.DBSlowSqlLogTime * 1000)
            {
                string log = $"耗时:{eventData.Duration.TotalSeconds}秒\r\n" +
                             $"语句:{command.CommandText}";
                _logger?.LogWarning(log);
            }
            return await base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override async Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            string log = $"异常:{eventData.Exception.Message}\r\n" +
                         $"语句:{command.CommandText}";
            _logger?.LogError(log);
            await base.CommandFailedAsync(command, eventData, cancellationToken);
        }
    }
}