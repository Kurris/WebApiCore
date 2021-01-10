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

namespace WebApiCore.Data.EF.DataBase
{
    /// <summary>
    /// 数据库操作实现抽象基类
    /// </summary>
    internal abstract class BaseDatabaseImp : IDataBaseOperation
    {

        internal BaseDatabaseImp(string provider, string connStr)
        {
            DbContext = new MyDbContext(provider, connStr);
        }

        #region 数据库级别操作

        public IDataBaseOperation GetIDataBaseOperation()
        {
            return this;
        }
        public DbContext DbContext { get; }
        public IDbContextTransaction DbContextTransaction { get; private set; }


        public virtual async Task<IDataBaseOperation> BeginTransAsync()
        {
            var connection = DbContext.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed) await connection.OpenAsync();

            DbContextTransaction = await DbContext.Database.BeginTransactionAsync();
            return this;
        }
        public virtual async Task<int> CommitTransAsync()
        {
            try
            {
                int commitResult = await DbContext.SaveChangesAsync();
                if (DbContextTransaction != null) await DbContextTransaction.CommitAsync();
                await this.CloseAsync();
                return commitResult;
            }
            catch
            {
                //异常抛出,如果存在事务,那么此时DbContext尚未释放
                throw;
            }
            finally
            {
                if (DbContextTransaction == null)
                {
                    await this.CloseAsync();
                }
            }
        }
        public virtual async Task RollbackTransAsync()
        {
            if (DbContextTransaction != null)
            {
                await DbContextTransaction.RollbackAsync();
                await DbContextTransaction.DisposeAsync();
                await this.CloseAsync();
            }
        }

#pragma warning disable CS1998 
        public virtual async Task<IDataBaseOperation> CreateSavepointAsync(string name)
        {
            throw new NotImplementedException("dotnet core 3.1 没有实现该功能");

            //IDataBaseOperation operation = DbContextTransaction == null
            //                        ? await BeginTransAsync()
            //                        : this;

            //await DbContextTransaction.CreateSavepointAsync(name);

            //return operation;
        }

        public virtual async Task RollbackToSavepointAsync(string name)
        {
            throw new NotImplementedException("dotnet core 3.1 没有实现该功能");

            //if (DbContextTransaction != null)
            //{
            //    await DbContextTransaction.RollbackToSavepointAsync(name);
            //}
        }
#pragma warning restore CS1998 

        public virtual async Task CloseAsync()
        {
            await DbContext.DisposeAsync();
        }

        #endregion

        #region 转换AsNoTracking/AsQueryable
        public virtual IQueryable<T> AsNoTracking<T>() where T : BaseEntity => this.DbContext.Set<T>().AsNoTracking();
        public virtual IQueryable<T> AsQueryable<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity =>
            predicate == null
                    ? this.AsQueryable<T>()
                    : this.AsQueryable<T>().Where(predicate);
        public virtual IQueryable<T> AsQueryable<T>() where T : BaseEntity => this.DbContext.Set<T>().AsQueryable();
        #endregion

        #region 查找主键
        public (string key, int value) FindPrimaryKeyValue<T>(T t) where T : BaseEntity
        {
            var entityType = this.DbContext.Model.FindEntityType(typeof(T));
            IKey key = entityType.FindPrimaryKey();
            var propInfo = key.Properties[0].PropertyInfo;
            return (propInfo.Name, Convert.ToInt32(propInfo.GetValue(t)));
        }

        public (string table, string key) FindPrimaryKeyWithTable<T>() where T : BaseEntity
        {
            var entityType = this.DbContext.Model.FindEntityType(typeof(T));
            string tableName = entityType.GetTableName();
            IKey key = entityType.FindPrimaryKey();

            return (tableName, key.Properties[0].PropertyInfo.Name);
        }

        public string FindPrimaryKey<T>() where T : BaseEntity
        {
            var entityType = this.DbContext.Model.FindEntityType(typeof(T));
            IKey key = entityType.FindPrimaryKey();
            return key.Properties[0].PropertyInfo.Name;
        }
        #endregion

        #region Sql操作
        public virtual async Task<int> RunSqlAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            if (keyValues != null)
                await DbContext.Database.ExecuteSqlRawAsync(strSql, new DbParameterBuilder(this.DbContext).AddParams(keyValues).GetParams());
            else
                await DbContext.Database.ExecuteSqlRawAsync(strSql);

            return await GetOperationReuslt();
        }
        public virtual async Task<int> RunSqlInterAsync(FormattableString strSql)
        {
            await DbContext.Database.ExecuteSqlInterpolatedAsync(strSql);
            return await GetOperationReuslt();
        }

#pragma warning disable CS1998
        public virtual async Task<int> ExecProcAsync(string procName, IDictionary<string, object> keyValues = null)
        {
            throw new NotImplementedException("请在派生类中实现");
        }
#pragma warning restore CS1998


        public virtual async Task<DataTable> GetTableAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            DataTable dt = await new DBHelper(this.DbContext).GetDataTable(strSql, keyValues);
            return dt;
        }
        public virtual async Task<IDataReader> GetReaderAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            IDataReader reader = await new DBHelper(this.DbContext).GetDataReader(strSql, keyValues);
            return reader;
        }
        public virtual async Task<object> GetScalarAsync(string strSql, IDictionary<string, object> keyValues = null)
        {
            object obj = await new DBHelper(this.DbContext).GetScalar(strSql, keyValues);
            return obj;
        }

        #endregion

        #region 删除

        public virtual async Task<int> DeleteAsync<T>(T entity) where T : BaseEntity
        {
            DbContext.Set<T>().Remove(entity);
            return await GetOperationReuslt();
        }
        public virtual async Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            DbContext.Set<T>().RemoveRange(entities);
            return await GetOperationReuslt();
        }
        public virtual async Task<int> DeleteAsync<T>(int keyValue) where T : BaseEntity
        {
            var (tableName, fieldKey) = FindPrimaryKeyWithTable<T>();

            return await this.RunSqlAsync($"Delete From {tableName} where {fieldKey}={keyValue};");
        }
        public virtual async Task<int> DeleteAsync<T>(IEnumerable<int> keyValues) where T : BaseEntity
        {
            var (tableName, fieldKey) = FindPrimaryKeyWithTable<T>();

            StringBuilder sb = new StringBuilder(keyValues.Count() + 1);
            sb.Append($"Delete From {tableName} \r\n where 1=1 and ( ");
            sb.AppendJoin(" or ", keyValues.Select(x => $" {fieldKey} = {x} "));
            sb.Append(" );");

            return await this.RunSqlAsync(sb.ToString());
        }
        public virtual async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            string tableName = FindPrimaryKeyWithTable<T>().table;

            ConditionBuilderVisitor visitor = new ConditionBuilderVisitor();
            visitor.Visit(predicate);

            return await this.RunSqlAsync($"Delete From {tableName} {visitor.CombineWithWhere()}");
        }

        #endregion

        #region 分页查询
        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sortColumn, bool isAsc, int pageSize, int pageIndex) where T : BaseEntity
        {
            var tempData = this.AsQueryable<T>();
            return await this.FindListAsync(tempData, sortColumn, isAsc, pageSize, pageIndex);
        }
        public virtual async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> condition, string sortColumn,
                                                                                     bool isAsc, int pageSize, int pageIndex) where T : BaseEntity
        {
            var tempData = this.AsQueryable(condition);
            return await this.FindListAsync(tempData, sortColumn, isAsc, pageSize, pageIndex);
        }
        private async Task<(int total, IEnumerable<T> list)> FindListAsync<T>(IQueryable<T> tmpdata, string sortColumn, bool isAsc,
                                                                              int pageSize, int pageIndex) where T : BaseEntity
        {
            tmpdata = DBExtension.PaginationSort(tmpdata, sortColumn, isAsc);

            var list = await tmpdata.ToListAsync();
            if (list?.Count > 0)
            {
                var currentData = list.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
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

        #endregion

        #region 更新
        public virtual async Task<int> UpdateAsync<T>(T entity, bool updateAll = false) where T : BaseEntity
        {
            if (updateAll)
            {
                this.DbContext.Update(entity);
            }
            else
            {
                DBExtension.RecursionAttach(this.DbContext, entity);
            }

            return await GetOperationReuslt();
        }
        public virtual async Task<int> UpdateAsync<T>(IEnumerable<T> entities, bool updateAll = false) where T : BaseEntity
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, updateAll);
            }

            return await GetOperationReuslt();
        }
        public virtual async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> wherePredicate, params Expression<Func<T, bool>>[] setPredicates) where T : BaseEntity
        {

            string tableName = FindPrimaryKeyWithTable<T>().table;

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
        #endregion

        #region 添加
        public virtual async Task<int> AddAsync<T>(T entity) where T : BaseEntity
        {
            this.DbContext.Set<T>().Add(entity);
            return await GetOperationReuslt();
        }
        public virtual async Task<int> AddAsync<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            await this.DbContext.Set<T>().AddRangeAsync(entities);
            return await GetOperationReuslt();
        }
        #endregion

        #region 查询
        public virtual async Task<T> FindAsync<T>(params object[] keyValues) where T : BaseEntity
        {
            var t = await DbContext.Set<T>().FindAsync(keyValues);
            return t;
        }
        public virtual async Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            var t = await DbContext.Set<T>().FirstOrDefaultAsync(predicate);
            return t;
        }
        public virtual async Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> predicate = null) where T : BaseEntity
        {

            IEnumerable<T> et = predicate == null ? await DbContext.Set<T>().ToListAsync()
                                                  : await DbContext.Set<T>().Where(predicate).ToListAsync();
            return et;
        }
        public virtual async Task<IEnumerable<T>> FindListByOrderAsync<T>(Expression<Func<T, object>> predicate, bool isAsc) where T : BaseEntity
        {
            IEnumerable<T> et = isAsc ? await DbContext.Set<T>().OrderBy(predicate).ToListAsync()
                                      : await DbContext.Set<T>().OrderByDescending(predicate).ToListAsync();
            return et;
        }

        #endregion


        /// <summary>
        /// 获取操作结果
        /// </summary>
        /// <returns>返回受影响数 <see cref="int"/></returns>
        protected async Task<int> GetOperationReuslt()
        {
            return DbContextTransaction == null //如果没有事务
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
