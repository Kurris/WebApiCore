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
    internal abstract class BaseDatabase : IDataBaseOperation
    {

        internal BaseDatabase(string provider, string connStr)
        {
            _dbContext = new MyDbContext(provider, connStr);
        }

        public IDataBaseOperation GetIDataBaseOperation()
        {
            return this;
        }

        private readonly DbContext _dbContext = null;
        private IDbContextTransaction _dbContextTransaction = null;

        public DbContext DbContext { get => _dbContext; }
        public IDbContextTransaction DbContextTransaction { get => _dbContextTransaction; }



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



        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return this._dbContext.Set<T>().AsQueryable();
        }



        public virtual async Task<int> RunSqlAsync(string strSql, params DbParameter[] dbParameter)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
            return await GetReuslt();
        }
        public virtual async Task<int> RunSqlInterAsync(FormattableString strSql)
        {
            await _dbContext.Database.ExecuteSqlInterpolatedAsync(strSql);
            return await GetReuslt();
        }
        public virtual async Task<int> ExecProcAsync(string procName, params DbParameter[] dbParameter)
        {
            await this.RunSqlAsync($"EXEC {procName}", dbParameter);
            return await GetReuslt();
        }



        public virtual async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            return await GetReuslt();
        }
        public virtual async Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : class
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return await GetReuslt();
        }
        public virtual async Task<int> DeleteAsync<T>(int keyValue) where T : class
        {
            IEntityType entityType = _dbContext.Set<T>().EntityType;
            if (entityType == null)
            {
                throw new NotSupportedException($"类型{entityType.Name}不符合跟踪要求!");
            }
            string tableNae = entityType.GetTableName();
            IKey key = entityType.FindPrimaryKey();
            string fieldKey = key.Properties[0].Name;

            return await this.RunSqlAsync($"Delete From {tableNae} where {fieldKey}={keyValue};");
        }
        public virtual async Task<int> DeleteAsync<T>(IEnumerable<int> keyValues) where T : class
        {
            IEntityType entityType = _dbContext.Set<T>().EntityType;
            if (entityType == null)
            {
                throw new ArgumentException($"类型{entityType.Name}不符合跟踪要求!");
            }
            string tableName = entityType.GetTableName();
            IKey key = entityType.FindPrimaryKey();
            string fieldKey = key.GetName();

            StringBuilder sb = new StringBuilder(keyValues.Count() + 1);
            sb.Append($"Delete From {tableName} \r\n where 1=1 and ( ");
            sb.AppendJoin(" or ", keyValues.Select(x => $" {fieldKey} = {x} "));
            sb.Append(" );");

            return await this.RunSqlAsync(sb.ToString());
        }
        public virtual async Task<int> DeleteAsync<T>(string propName, object propValue) where T : class
        {
            IEntityType entityType = _dbContext.Set<T>().EntityType;
            if (entityType == null)
            {
                throw new ArgumentException($"类型{entityType.Name}不符合跟踪要求!");
            }
            string tableName = entityType.GetTableName();

            return await this.RunSqlAsync($"Delete From {tableName} where {propName}='{propValue}';");
        }


        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sortColumn, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            var tempData = _dbContext.Set<T>().AsQueryable();
            return await this.FindListAsync<T>(tempData, sortColumn, isAsc, pageSize, pageIndex);
        }
        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> condition, string sortColumn,
                                                                                     bool isAsc, int pageSize, int pageIndex) where T : class
        {
            var tempData = _dbContext.Set<T>().Where(condition);
            return await this.FindListAsync<T>(tempData, sortColumn, isAsc, pageSize, pageIndex);
        }
#pragma warning disable CA1822 // 将成员标记为 static
        private async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(IQueryable<T> tmpdata, string sortColumn, bool isAsc,
#pragma warning restore CA1822 // 将成员标记为 static
                                                                              int pageSize, int pageIndex) where T : class
        {
            tmpdata = DBExtension.PaginationSort<T>(tmpdata, sortColumn, isAsc);

            var list = await tmpdata.ToListAsync();
            if (list?.Count > 0)
            {
                var currentData = list.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
                return (list.Count, currentData);
            }
            else
            {
                return (0, new List<T>());
            }
        }



        public virtual async Task<int> UpdateAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Update(entity);
            return await GetReuslt();
        }
        public virtual async Task<int> UpdateAsync<T>(IEnumerable<T> entities) where T : class
        {
            this._dbContext.Set<T>().UpdateRange(entities);
            return await GetReuslt();
        }


        public virtual async Task<int> AttachAsync<T>(T entity, params string[] props) where T : class
        {
            var entityType = this._dbContext.Set<T>().Attach(entity);
            foreach (var prop in props)
            {
                entityType.Property(prop).IsModified = true;
            }
            return await GetReuslt();
        }
        public virtual async Task<int> AttachAsync<T>(IEnumerable<T> entities, params string[] props) where T : class
        {
            foreach (var entity in entities)
            {
                var entityType = this._dbContext.Set<T>().Attach(entity);
                foreach (var prop in props)
                {
                    entityType.Property(prop).IsModified = true;
                }
            }

            return await GetReuslt();
        }



        public virtual async Task<int> AddAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Add(entity);
            return await GetReuslt();
        }
        public virtual async Task<int> AddAsync<T>(IEnumerable<T> entities) where T : class
        {
            await this._dbContext.Set<T>().AddRangeAsync(entities);
            return await GetReuslt();
        }



        public virtual async Task<T> FindAsync<T>(params object[] keyValues) where T : class
        {
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }
        public virtual async Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _dbContext.Set<T>().FindAsync(predicate);
        }
        public virtual async Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            if (predicate == null)
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> FindListByOrderAsync<T>(Expression<Func<T, object>> predicate, bool isAsc) where T : class
        {
            if (isAsc)
            {
                return await _dbContext.Set<T>().OrderBy(predicate).ToListAsync();
            }
            return await _dbContext.Set<T>().OrderByDescending(predicate).ToListAsync();
        }



        public virtual async Task<DataTable> GetTableAsync(string strSql, params DbParameter[] dbParameter)
        {
            return await DBHelper.GetInstance(this._dbContext).GetDataTable(strSql, dbParameter);
        }
        public virtual async Task<IDataReader> GetReaderAsync(string strSql, params DbParameter[] dbParameter)
        {
            return await DBHelper.GetInstance(this._dbContext).GetDataReader(strSql, dbParameter);
        }
        public virtual async Task<object> GetScalarAsync(string strSql, params DbParameter[] dbParameter)
        {
            return await DBHelper.GetInstance(this._dbContext).GetScalar(strSql, dbParameter);
        }




        private async Task<int> GetReuslt()
        {
            return _dbContextTransaction == null//如果没有事务
                ? await this.CommitTransAsync() //那么立即提交
                : 0;                            //否则返回0;
        }




        public virtual async ValueTask DisposeAsync()
        {
            await this.CloseAsync();
        }
        public virtual async void Dispose()
        {
            await this.CloseAsync();
        }


    }
}
