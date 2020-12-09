using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public interface IDataBaseOperation
    {
        #region Property

        /// <summary>
        /// 数据库当前上下文
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// 数据库当前上下文事务
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; }

        #endregion

        #region DataBase Methods

        /// <summary>
        /// 获取当前数据库类型操作
        /// </summary>
        /// <returns><see cref="IDataBaseOperation"/></returns>
        IDataBaseOperation GetIDataBaseOperation();

        /// <summary>
        /// 确保数据库被删除
        /// </summary>
        /// <returns><see cref="IDataBaseOperation"/></returns>
        Task<IDataBaseOperation> EnsureDeletedAsync();

        /// <summary>
        /// 确保数据库被创建
        /// </summary>
        /// <returns><see cref="IDataBaseOperation"/></returns>
        Task<IDataBaseOperation> EnsureCreatedAsync();

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns><see cref="IDataBaseOperation"/></returns>
        Task<IDataBaseOperation> BeginTransAsync();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns>影响行数<see cref="int"/></returns>
        Task<int> CommitTransAsync();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task RollbackTransAsync();

        /// <summary>
        /// 回滚事务到保存点
        /// </summary>
        /// <param name="name">保存点名称</param>
        /// <returns><see cref="Task"/></returns>
        Task RollbackToSavepointAsync(string name);

        /// <summary>
        /// 创建事务保存点
        /// </summary>
        /// <param name="name">保存点名称</param>
        /// <returns><see cref="IDataBaseOperation"/></returns>
        Task<IDataBaseOperation> CreateSavepointAsync(string name);

        /// <summary>
        /// 关闭当前数据库上下文对象,并且释放
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task CloseAsync();

        #endregion

        #region T-SQL 

        Task<int> ExecuteSqlRawAsync(string strSql);
        Task<int> ExecuteSqlRawAsync(string strSql, params DbParameter[] dbParameter);
        Task<int> ExecuteSqlInterpolatedAsync(FormattableString strSql);
        Task<int> ExecuteByProcAsync(string procName);
        Task<int> ExecuteByProcAsync(string procName, DbParameter[] dbParameter);

        #endregion

        #region Insert

        Task<int> InsertAsync<T>(T entity) where T : class;
        Task<int> InsertAsync(params object[] entities);
        Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : class;

        #endregion

        #region Update

        Task<int> UpdateAsync<T>(IEnumerable<T> entities) where T : class;
        Task<int> UpdateAllFieldAsync<T>(T entity) where T : class;

        #endregion

        #region Delete

        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : class;
        Task<int> DeleteAsync<T>(int id) where T : class;
        Task<int> DeleteAsync<T>(IEnumerable<int> id) where T : class;

        #endregion

        #region Find

        #region EF Find

        #region Find Single 
        Task<T> FindEntityAsync<T>(object KeyValue) where T : class;
        Task<T> FindEntityAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();

        #endregion

        #region Find Entities

        Task<IEnumerable<T>> FindListAsync<T>() where T : class, new();
        Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        Task<IEnumerable<T>> FindListASCAsync<T>(Expression<Func<T, object>> predicate) where T : class, new();
        Task<IEnumerable<T>> FindListDESCAsync<T>(Expression<Func<T, object>> predicate) where T : class, new();

        #endregion

        #region Pagination

        Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();
        Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();

        #endregion



        #endregion

        #region T-SQL Find

        #region Find DataTable
        Task<DataTable> FindTableAsync(string strSql);
        Task<DataTable> FindTableAsync(string strSql, DbParameter[] dbParameter);

        #endregion

        #region Find Object

        Task<object> FindObjectAsync(string strSql);
        Task<object> FindObjectAsync(string strSql, DbParameter[] dbParameter);

        #endregion

        #endregion

        #endregion

        #region IQueryable

        IQueryable<T> IQueryableAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();

        #endregion
    }
}
