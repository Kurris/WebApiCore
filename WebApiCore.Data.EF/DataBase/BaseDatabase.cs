using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using WebApiCore.Data.EF.DataBase.Extension;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Data.EF.DataBase
{
    /// <summary>
    /// 数据库操作实现抽象基类
    /// </summary>
    internal abstract class BaseDatabaseImp : IDataBaseOperation
    {

        internal BaseDatabaseImp(string provider, string connStr)
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
            throw new NotImplementedException(".net core 3.1");

            //IDataBaseOperation operation = _dbContextTransaction == null
            //                        ? await BeginTransAsync()
            //                        : this;

            //await _dbContextTransaction.CreateSavepointAsync(name);

            //return operation;
        }
        public virtual async Task RollbackToSavepointAsync(string name)
        {
            throw new NotImplementedException(".net core 3.1");

            //if (_dbContextTransaction != null)
            //{
            //    await _dbContextTransaction.RollbackToSavepointAsync(name);
            //}
        }
        public virtual async Task CloseAsync()
        {
            await _dbContext.DisposeAsync();
        }


        public virtual IQueryable<T> AsNoTracking<T>() where T : BaseEntity
        {
            return this._dbContext.Set<T>().AsNoTracking(); ;
        }
        public virtual IQueryable<T> AsQueryable<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            return this.AsQueryable<T>().Where(predicate);
        }
        public virtual IQueryable<T> AsQueryable<T>() where T : BaseEntity
        {
            return this._dbContext.Set<T>().AsQueryable();
        }



        public virtual async Task<int> RunSqlAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            if (keyValues != null)
            {
                DbParameterBuilder dbParameterBuilder = new DbParameterBuilder(this._dbContext);
                foreach (var item in keyValues)
                {
                    dbParameterBuilder.SetParams(item.Key, item.Value);
                }
                await _dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameterBuilder.GetParams());
            }
            else
            {
                await _dbContext.Database.ExecuteSqlRawAsync(strSql);
            }

            return await GetReuslt();
        }
        public virtual async Task<int> RunSqlInterAsync(FormattableString strSql)
        {
            await _dbContext.Database.ExecuteSqlInterpolatedAsync(strSql);
            return await GetReuslt();
        }
        public virtual async Task<int> ExecProcAsync(string procName, IDictionary<string, object> keyValues = null)
        {
            throw new NotImplementedException("请在派生类中实现");
        }



        public virtual async Task<int> DeleteAsync<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            return await GetReuslt();
        }
        public virtual async Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            _dbContext.Set<T>().RemoveRange(entities);
            return await GetReuslt();
        }
        public virtual async Task<int> DeleteAsync<T>(int keyValue) where T : BaseEntity
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));
            string tableNae = entityType.GetTableName();
            string fieldKey = FindPrimaryKey<T>();

            return await this.RunSqlAsync($"Delete From {tableNae} where {fieldKey}={keyValue};");
        }
        public virtual async Task<int> DeleteAsync<T>(IEnumerable<int> keyValues) where T : BaseEntity
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));
            string tableName = entityType.GetTableName();
            string fieldKey = FindPrimaryKey<T>();

            StringBuilder sb = new StringBuilder(keyValues.Count() + 1);
            sb.Append($"Delete From {tableName} \r\n where 1=1 and ( ");
            sb.AppendJoin(" or ", keyValues.Select(x => $" {fieldKey} = {x} "));
            sb.Append(" );");

            return await this.RunSqlAsync(sb.ToString());
        }
        public virtual async Task<int> DeleteAsync<T>(string propName, object propValue) where T : BaseEntity
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));
            string tableName = entityType.GetTableName();
            return await this.RunSqlAsync($"Delete From {tableName} where {propName}=@" + propName + ";", new Dictionary<string, object>()
            {
                [propName] = propValue
            });
        }
        public virtual async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));
            string tableName = entityType.GetTableName();

            ConditionBuilderVisitor visitor = new ConditionBuilderVisitor();
            visitor.Visit(predicate);

            return await this.RunSqlAsync($"Delete From {tableName} {visitor.CombineWithWhere()}");
        }



        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sortColumn, bool isAsc, int pageSize, int pageIndex) where T : BaseEntity
        {
            var tempData = _dbContext.Set<T>().AsQueryable();
            return await this.FindListAsync<T>(tempData, sortColumn, isAsc, pageSize, pageIndex);
        }
        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> condition, string sortColumn,
                                                                                     bool isAsc, int pageSize, int pageIndex) where T : BaseEntity
        {
            var tempData = _dbContext.Set<T>().Where(condition);
            return await this.FindListAsync<T>(tempData, sortColumn, isAsc, pageSize, pageIndex);
        }
        private async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(IQueryable<T> tmpdata, string sortColumn, bool isAsc,
                                                                              int pageSize, int pageIndex) where T : BaseEntity
        {
            tmpdata = DBExtension.PaginationSort(tmpdata, sortColumn, isAsc);

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
        public virtual async Task<(int total, DataTable)> FindTableAsync(string strSql, IDictionary<string, object> dbParameters, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            if (pageIndex == 0) pageIndex = 1;
            int numLeft = (pageIndex - 1) * pageSize + 1;
            int numRight = (pageIndex) * pageSize;

            string OrderBy;

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                    OrderBy = " ORDER BY " + sort;
                else
                    OrderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
            }
            else
            {
                OrderBy = "ORDERE BY (SELECT 0)";
            }

            string sql = $@"
SELECT * FROM 
(SELECT  ROW_NUMBER () OVER({OrderBy}) AS ROWNUM, * FROM ({strSql})t1 ) T2
WHERE T2.ROWNUM BETWEEN  {numLeft} AND {numRight};";

            object res = await this.GetScalarAsync($"SELECT COUNT(1) FROM ({strSql}) t", dbParameters);
            int total = Convert.ToInt32(res);
            return (total, await this.GetTableAsync(sql, dbParameters));
        }



        public virtual async Task<int> UpdateAsync<T>(T entity, bool updateAll = false) where T : BaseEntity
        {
            if (updateAll)
            {
                this._dbContext.Update(entity);
            }
            else
            {
                DBExtension.RecursionAttach(this._dbContext, entity);
            }

            return await GetReuslt();
        }
        public virtual async Task<int> UpdateAsync<T>(IEnumerable<T> entities, bool updateAll = false) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, updateAll);
            }

            return await GetReuslt();
        }
        public virtual async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> wherePredicate, params Expression<Func<T, bool>>[] setPredicates) where T : BaseEntity
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));

            string tableName = entityType.GetTableName();

            ConditionBuilderVisitor whereCondition = new ConditionBuilderVisitor();
            whereCondition.Visit(wherePredicate);
            string whereString = whereCondition.CombineWithWhere();

            string setStrings = string.Join(",", setPredicates.Select(x =>
             {
                 ConditionBuilderVisitor setCondition = new ConditionBuilderVisitor();
                 setCondition.Visit(x);
                 return setCondition.Combine();

             })).TrimStart('(', ' ').TrimEnd(')', ' ');


            string sql = $@"UPDATE {tableName} SET {setStrings} {whereString}";

            return await this.RunSqlAsync(sql);
        }



        public virtual async Task<int> AddAsync<T>(T entity) where T : BaseEntity
        {
            this._dbContext.Set<T>().Add(entity);
            return await GetReuslt();
        }
        public virtual async Task<int> AddAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            await this._dbContext.Set<T>().AddRangeAsync(entities);
            return await GetReuslt();
        }



        public virtual async Task<T> FindAsync<T>(params object[] keyValues) where T : BaseEntity
        {
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }
        public virtual async Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public virtual async Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> predicate = null) where T : BaseEntity
        {
            if (predicate == null)
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> FindListByOrderAsync<T>(Expression<Func<T, object>> predicate, bool isAsc) where T : BaseEntity
        {
            if (isAsc)
            {
                return await _dbContext.Set<T>().OrderBy(predicate).ToListAsync();
            }
            return await _dbContext.Set<T>().OrderByDescending(predicate).ToListAsync();
        }



        public virtual async Task<DataTable> GetTableAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            DBHelper helper = new DBHelper(this._dbContext);
            if (keyValues != null)
            {
                foreach (var item in keyValues)
                {
                    helper.SetParam(item.Key, item.Value);
                }
            }
            return await helper.GetDataTable(strSql, helper.GetParams());
        }
        public virtual async Task<IDataReader> GetReaderAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            DBHelper helper = new DBHelper(this._dbContext);
            if (keyValues != null)
            {
                foreach (var item in keyValues)
                {
                    helper.SetParam(item.Key, item.Value);
                }
            }
            return await helper.GetDataReader(strSql, helper.GetParams());
        }
        public virtual async Task<object> GetScalarAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            DBHelper helper = new DBHelper(this._dbContext);
            if (keyValues != null)
            {
                foreach (var item in keyValues)
                {
                    helper.SetParam(item.Key, item.Value);
                }
            }
            return await helper.GetScalar(strSql, helper.GetParams());
        }




        protected async Task<int> GetReuslt()
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

        public (string key, int value) FindPrimaryKeyValue<T>(T t) where T : BaseEntity
        {
            var entityType = this._dbContext.Model.FindEntityType(typeof(T));
            IKey key = entityType.FindPrimaryKey();
            var propInfo = key.Properties[0].PropertyInfo;
            return (propInfo.Name, propInfo.GetValue(t).ParseToInt());
        }
        public string FindPrimaryKey<T>() where T : BaseEntity
        {
            var entityType = this._dbContext.Model.FindEntityType(typeof(T));
            IKey key = entityType.FindPrimaryKey();
            return key.Properties[0].PropertyInfo.Name;
        }
    }
}
