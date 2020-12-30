using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApiCore.Data.EF.DataBase
{
    internal sealed class DBHelper
    {
        private readonly DbContext _dbContext = null;
        private readonly DbProviderFactory _dbProviderFactory = null;

        internal DBHelper(DbContext dbContext)
        {
            _dbContext = dbContext;

            _dbProviderFactory = DbProviderFactories.GetFactory(_dbContext.Database.GetDbConnection());
        }

        /// <summary>
        /// 创建DbCommand对象,打开连接,打开事务
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>IDbCommand</returns>
        private async Task<DbCommand> CreateDbCommand(string sql, DbParameter[] parameters)
        {
            try
            {
                var dbConnection = _dbContext.Database.GetDbConnection();
                var cmd = dbConnection.CreateCommand();

                cmd.CommandText = sql;
                cmd.Transaction = _dbContext.Database.CurrentTransaction.GetDbTransaction();

                if (parameters != null && parameters.Length != 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    await cmd.Connection.OpenAsync();
                }

                return cmd;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>返回受影响行数<see cref="int"/></returns>
        internal async Task<int> RunSql(string sql)
        {
            return await RunSql(sql, null);
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的行数<see cref="int"/></returns>
        internal async Task<int> RunSql(string sql, DbParameter[] parameters)
        {
            using (DbCommand cmd = await CreateDbCommand(sql, parameters))
            {
                try
                {
                    return await cmd.ExecuteNonQueryAsync();
                }
                catch
                {
                    await cmd.DisposeAsync();
                    throw;
                }
                finally
                {
                    if (cmd.Transaction == null)
                    {
                        await this.CloseAsync();
                    }
                }
            }
          
        }

        /// <summary>
        /// 执行查询语句,单行
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DbDataReader对象</returns>
        internal async Task<IDataReader> GetDataReader(string sql)
        {
            return await GetDataReader(sql, null);
        }

        /// <summary>
        /// 执行查询语句,单行
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="commandType">执行命令类型</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DbDataReader对象</returns>
        internal async Task<IDataReader> GetDataReader(string sql, DbParameter[] parameters)
        {
            using (DbCommand cmd = await CreateDbCommand(sql, parameters))
            {
                try
                {
                    return await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
                }
                catch
                {
                    await cmd.DisposeAsync();
                    throw;
                }
                finally
                {
                    if (cmd.Transaction == null)
                    {
                        await this.CloseAsync();
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DataTable</returns>
        internal async Task<DataTable> GetDataTable(string sql)
        {
            return await GetDataTable(sql, null);
        }

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns><see cref="DataTable"</returns>
        internal async Task<DataTable> GetDataTable(string sql, DbParameter[] parameters)
        {
            using (DbCommand cmd = await CreateDbCommand(sql, parameters))
            {
                try
                {
                    using var adapter = _dbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
                catch
                {
                    await cmd.DisposeAsync();
                    throw;
                }
                finally
                {
                    if (_dbContext.Database.CurrentTransaction == null)
                    {
                        await this.CloseAsync();
                    }
                }
            }
        }


        /// <summary>
        /// 执行一个查询语句，返回查询结果的首行首列
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>首行首列</returns>
        internal async Task<object> GetScalar(string sql)
        {
            return await GetScalar(sql, null);
        }

        /// <summary>
        /// 执行一个查询语句，返回查询结果的首行首列
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>首行首列object</returns>
        internal async Task<object> GetScalar(string sql, DbParameter[] parameters)
        {
            using (DbCommand cmd = await CreateDbCommand(sql, parameters))
            {
                try
                {
                    return await cmd.ExecuteScalarAsync();
                }
                catch
                {
                    await cmd.DisposeAsync();
                    throw;
                }
                finally
                {
                    if (cmd.Transaction == null)
                    {
                        await this.CloseAsync();
                    }
                }
            }

        }

        private readonly DbParameterBuilder _dbParameterBuilder = new DbParameterBuilder();

        internal DbParameterBuilder SetParam(string name, object value)
        {
            var para = _dbProviderFactory.CreateParameter();
            para.ParameterName = name;
            para.Value = value;

            _dbParameterBuilder.SetParams(para);

            return _dbParameterBuilder;
        }

        internal DbParameter[] GetParams() => _dbParameterBuilder.GetParams();

        /// <summary>
        /// 释放上下文对象
        /// </summary>
        /// <returns></returns>
#pragma warning disable CA1822 // 将成员标记为 static
        private async ValueTask CloseAsync()
#pragma warning restore CA1822 // 将成员标记为 static
        {
            await _dbContext.DisposeAsync();
        }
    }

    class DbParameterBuilder
    {
        internal DbParameterBuilder()
        {

        }

        internal DbParameterBuilder(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        private List<DbParameter> _dbParameters = new List<DbParameter>();
        private readonly DbContext _dbContext;
        private DbProviderFactory _dbProviderFactory = null;

        internal DbProviderFactory DbProviderFactory
        {
            get
            {
                if (_dbProviderFactory == null)
                {
                    _dbProviderFactory = DbProviderFactories.GetFactory(_dbContext.Database.GetDbConnection());
                }
                return _dbProviderFactory;
            }
        }

        internal DbParameter[] GetParams()
        {
            return _dbParameters.ToArray();
        }
        internal void SetParams(DbParameter dbParameter)
        {
            _dbParameters.Add(dbParameter);
        }

        internal DbParameterBuilder SetParams(string name, object value)
        {
            var para = DbProviderFactory.CreateParameter();
            para.ParameterName = name;
            para.Value = value;

            _dbParameters.Add(para);

            return this;
        }
    }
}