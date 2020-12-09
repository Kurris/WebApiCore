using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    public sealed class DBHelper
    {
        private DBHelper() { }

        private static DbContext _dbContext = null;
        private readonly static DBHelper _dbHelper = new DBHelper();

        public static DBHelper GetInstance(DbContext dbContext)
        {
            _dbContext = dbContext;

            return _dbHelper;
        }


        /// <summary>
        /// 创建DbCommand对象,打开连接,打开事务
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DbCommand</returns>
        private async Task<DbCommand> CreateDbCommand(string sql, DbParameter[] parameters)
        {
            try
            {
                DbConnection dbConnection = _dbContext.Database.GetDbConnection();
                DbCommand command = dbConnection.CreateCommand();

                command.CommandText = sql;

                if (parameters != null && parameters.Length != 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (command.Connection.State == ConnectionState.Closed)
                {
                    await command.Connection.OpenAsync();
                }

                return command;
            }
            catch
            {
                throw;
            }
            finally
            {
                await this.CloseAsync();
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>返回受影响行数<see cref="int"/></returns>
        public async Task<int> RunSql(string sql)
        {
            return await RunSql(sql, null);
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>受影响的行数<see cref="int"/></returns>
        public async Task<int> RunSql(string sql, DbParameter[] parameters)
        {
            using DbCommand cmd = await CreateDbCommand(sql, parameters);
            try
            {
                int iRes = await cmd.ExecuteNonQueryAsync();
                if (cmd.Transaction != null)
                {
                    await cmd.Transaction.CommitAsync();
                }
                await this.CloseAsync();
                return iRes;
            }
            catch
            {
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

        /// <summary>
        /// 执行查询语句,单行
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DbDataReader对象</returns>
        public async Task<IDataReader> GetDataReader(string sql)
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
        public async Task<IDataReader> GetDataReader(string sql, DbParameter[] parameters)
        {
            DbCommand cmd = await CreateDbCommand(sql, parameters);

            try
            {
                IDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (cmd.Transaction != null)
                {
                    await cmd.Transaction.CommitAsync();
                }
                await this.CloseAsync();
                return reader;
            }
            catch
            {
                cmd.Dispose();
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

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> GetDataTable(string sql)
        {
            return await GetDataTable(sql, null);
        }

        /// <summary>
        /// 执行查询语句,返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns><see cref="DataTable"</returns>
        public async Task<DataTable> GetDataTable(string sql, DbParameter[] parameters)
        {
            using DbCommand cmd = await CreateDbCommand(sql, parameters);
            using IDataReader reader = await cmd.ExecuteReaderAsync();
            try
            {
                DataTable dt = reader.GetSchemaTable();
                object[] drs = new object[reader.FieldCount];
                dt.BeginLoadData();
                while (reader.Read())
                {
                    reader.GetValues(drs);
                    dt.Rows.Add(drs);
                }
                dt.EndLoadData();

                if (cmd.Transaction != null)
                {
                    await cmd.Transaction.CommitAsync();
                }

                await this.CloseAsync();

                return dt;
            }
            catch
            {
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


        /// <summary>
        /// 执行一个查询语句，返回查询结果的首行首列
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <returns>首行首列</returns>
        public async Task<object> GetScalar(string sql)
        {
            return await GetScalar(sql, null);
        }

        /// <summary>
        /// 执行一个查询语句，返回查询结果的首行首列
        /// </summary>
        /// <param name="sql">执行语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>首行首列object</returns>
        public async Task<object> GetScalar(string sql, DbParameter[] parameters)
        {
            using DbCommand cmd = await CreateDbCommand(sql, parameters);
            try
            {
                object obj = await cmd.ExecuteScalarAsync();

                if (cmd.Transaction != null)
                {
                    await cmd.Transaction.CommitAsync();
                }
                await this.CloseAsync();
                return obj;
            }
            catch
            {
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
}