using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.BlogInfos;

namespace WebApiCore.EF.DataBase
{
    public abstract class BaseDatabase : IDataBaseOperation
    {
        public BaseDatabase()
        {
            _dbContext = new MyDbContext();
        }

        public BaseDatabase(string connStr)
        {
            _dbContext = new MyDbContext(connStr);
        }

        #region Fields
        private readonly DbContext _dbContext = null;
        private IDbContextTransaction _dbContextTransaction = null;
        #endregion


        public IDataBaseOperation GetDataBase()
        {
            return this;
        }

        #region Properties
        public DbContext DbContext { get => _dbContext; }
        public IDbContextTransaction DbContextTransaction { get => _dbContextTransaction; }
        #endregion

        #region Transcation Operate

        public virtual async Task<IDataBaseOperation> EnsureDeletedAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            return this;
        }
        public virtual async Task<IDataBaseOperation> EnsureCreatedAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            return this;
        }


        public virtual async Task<IDataBaseOperation> BeginTransAsync()
        {
            var conn = _dbContext.Database.GetDbConnection();
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            return this;
        }
        public virtual async Task<int> CommitTransAsync()
        {
            try
            {
                int taskResult = await _dbContext.SaveChangesAsync();

                if (_dbContextTransaction != null)
                {
                    await _dbContextTransaction.CommitAsync();
                }
                await this.CloseAsync();
                return taskResult;
            }
            catch
            {
                //throw exception
                throw;
            }
            finally
            {
                if (_dbContextTransaction == null)
                {
                    await this.CloseAsync();
                }
            }
        }
        public virtual async Task RollbackTransAsync()
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.RollbackAsync();
                await _dbContextTransaction.DisposeAsync();
                await this.CloseAsync();
            }
        }
        public virtual async Task<IDataBaseOperation> CreateSavepointAsync(string name)
        {
            IDataBaseOperation operation = _dbContextTransaction == null
                                    ? await BeginTransAsync()
                                    : this;

            await _dbContextTransaction.CreateSavepointAsync(name);

            return operation;
        }
        public virtual async Task RollbackToSavepointAsync(string name)
        {
            if (_dbContextTransaction != null)
            {
                await _dbContextTransaction.RollbackToSavepointAsync(name);
            }
        }
        public virtual async Task CloseAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion

        #region Sql Exec
        public virtual async Task<int> ExecuteSqlRawAsync(string strSql)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(strSql);
            return await GetReuslt();
        }
        public virtual async Task<int> ExecuteSqlRawAsync(string strSql, params DbParameter[] dbParameter)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
            return await GetReuslt();
        }
        public virtual async Task<int> ExecuteSqlInterpolatedAsync(FormattableString strSql)
        {
            await _dbContext.Database.ExecuteSqlInterpolatedAsync(strSql);
            return await GetReuslt();
        }
        public virtual async Task<int> ExecuteByProcAsync(string procName)
        {
            await this.ExecuteSqlRawAsync($"EXEC {procName}");
            return await GetReuslt();
        }
        public virtual async Task<int> ExecuteByProcAsync(string procName, DbParameter[] dbParameter)
        {
            await this.ExecuteSqlRawAsync(procName, dbParameter);
            return await GetReuslt();
        }

        #endregion

        #region Insert

        public virtual async Task<int> InsertAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Add(entity);
            return await GetReuslt();
        }

        public virtual async Task<int> InsertAsync(params object[] entities)
        {
            await _dbContext.AddRangeAsync(entities);
            return await GetReuslt();
        }

        public virtual async Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : class
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return await GetReuslt();
        }

        #endregion

        #region Delete
        public virtual async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            return await GetReuslt();
        }

        public virtual async Task<int> DeleteAsync(params object[] entities)
        {
            _dbContext.RemoveRange(entities);
            return await GetReuslt();
        }

        public virtual async Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : class
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return await GetReuslt();
        }

        public virtual async Task<int> DeleteAsync<T>(int id) where T : class
        {
            IEntityType entityType = _dbContext.Set<T>().EntityType;
            if (entityType == null)
            {
                throw new ArgumentException($"类型{entityType.Name}不符合跟踪要求!");
            }
            string tableNae = entityType.GetTableName();
            string fieldKey = "Id";

            return await this.ExecuteSqlRawAsync($"Delete From {tableNae} where {fieldKey}={id};");
        }

        public virtual async Task<int> DeleteAsync<T>(IEnumerable<int> ids) where T : class
        {
            IEntityType entityType = _dbContext.Set<T>().EntityType;
            if (entityType == null)
            {
                throw new ArgumentException($"类型{entityType.Name}不符合跟踪要求!");
            }
            string tableNae = entityType.GetTableName();
            string fieldKey = "Id";

            StringBuilder sb = new StringBuilder(ids.Count() + 1);
            sb.Append($"Delete From {tableNae} \r\n where 1=1 and ( ");
            sb.AppendJoin(" or ", ids.Select(x => $" {fieldKey} = {x} "));
            sb.Append(" );");

            return await this.ExecuteSqlRawAsync(sb.ToString());
        }

        public virtual async Task<int> DeleteAsync<T, TProp>(TProp propertyName, TProp propertyValue) where TProp : MemberInfo where T : class
        {
            IEntityType entityType = _dbContext.Set<T>().EntityType;
            if (entityType == null)
            {
                throw new ArgumentException($"类型{entityType.Name}不符合跟踪要求!");
            }
            string tableNae = entityType.GetTableName();

            return await this.ExecuteSqlRawAsync($"Delete From {tableNae} where {propertyName.Name}='{propertyValue}';");
        }

        #endregion



        public virtual async Task<T> FindEntityAsync<T>(object KeyValue) where T : class
        {
            return await _dbContext.Set<T>().FindAsync(KeyValue);
        }

        public virtual async Task<T> FindEntityAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> FindListAsync<T>() where T : class, new()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindListASCAsync<T>(Expression<Func<T, object>> predicate) where T : class, new()
        {
            return await _dbContext.Set<T>().OrderBy(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindListDESCAsync<T>(Expression<Func<T, object>> predicate) where T : class, new()
        {
            return await _dbContext.Set<T>().OrderByDescending(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<(int total, IEnumerable<T>)> FindListAsync<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual async Task<(int total, IEnumerable<T>)> FindListAsync<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual async Task<object> FindObjectAsync(string strSql)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<object> FindObjectAsync(string strSql, DbParameter[] dbParameter)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> FindObjectAsync<T>(string strSql) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual async Task<DataTable> FindTableAsync(string strSql)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<DataTable> FindTableAsync(string strSql, DbParameter[] dbParameter)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<(int total, DataTable)> FindTableAsync(string strSql, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<(int total, DataTable)> FindTableAsync(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }


        public virtual IQueryable<T> IQueryableAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public virtual async Task<int> UpdateAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Update(entity);
            return await GetReuslt();
        }

        public virtual async Task<int> UpdateAsync<T>(IEnumerable<T> entities) where T : class
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return await GetReuslt();
        }

        public virtual async Task<int> UpdateAllFieldAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Update(entity);
            return await GetReuslt();
        }


        private async Task<int> GetReuslt()
        {
            return _dbContextTransaction == null//当前事务
                ? await this.CommitTransAsync() //没有事务立即提交
                : 0;                            //有事务就返回0;
        }
    }
}
