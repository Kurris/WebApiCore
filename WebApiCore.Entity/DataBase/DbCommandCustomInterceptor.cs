using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using WebApiCore.Utils;

namespace WebApiCore.EF.DataBase
{
    /// <summary>
    /// 自定义数据库拦截器
    /// </summary>
    internal class DbCommandCustomInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<DbCommandCustomInterceptor> _logger = GlobalInvariant.ServiceProvider?.GetService<ILogger<DbCommandCustomInterceptor>>();

        public override Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalInvariant.SystemConfig?.DBConfig.DBSlowSqlLogTime * 1000)
            {
                string log = $"耗时:{eventData.Duration.TotalSeconds}秒\r\n" +
                             $"语句:{command.CommandText}";
                _logger?.LogWarning(log);
            }
            return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalInvariant.SystemConfig?.DBConfig.DBSlowSqlLogTime * 1000)
            {
                string log = $"耗时:{eventData.Duration.TotalSeconds}秒\r\n" +
                             $"语句:{command.CommandText}";
                _logger?.LogWarning(log);
            }
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalInvariant.SystemConfig?.DBConfig.DBSlowSqlLogTime * 1000)
            {
                string log = $"耗时:{eventData.Duration.TotalSeconds}秒\r\n" +
                             $"语句:{command.CommandText}";
                _logger?.LogWarning(log);
            }
            return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
        }

        public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            string log = $"异常:{eventData.Exception.Message}\r\n" +
                         $"语句:{command.CommandText}";
            _logger?.LogWarning(log);
            return base.CommandFailedAsync(command, eventData, cancellationToken);
        }
    }
}