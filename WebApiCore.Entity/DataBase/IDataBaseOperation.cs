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
        #region 属性

        public DbContext DbContext { get; }

        public IDbContextTransaction DbContextTransaction { get; }

        #endregion


        IDataBaseOperation GetIDataBaseOperation();
        Task<IDataBaseOperation> EnsureDeletedAsync();
        Task<IDataBaseOperation> EnsureCreatedAsync();

        #region 方法
        Task<IDataBaseOperation> BeginTransAsync();
        Task<int> CommitTransAsync();
        Task RollbackTransAsync();
        Task RollbackToSavepointAsync(string name);
        Task<IDataBaseOperation> CreateSavepointAsync(string name);
        Task CloseAsync();

        Task<int> ExecuteSqlRawAsync(string strSql);
        Task<int> ExecuteSqlRawAsync(string strSql, params DbParameter[] dbParameter);
        Task<int> ExecuteSqlInterpolatedAsync(FormattableString strSql);
        Task<int> ExecuteByProcAsync(string procName);
        Task<int> ExecuteByProcAsync(string procName, DbParameter[] dbParameter);
        Task<int> InsertAsync<T>(T entity) where T : class;
        Task<int> InsertAsync(params object[] entities);
        Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : class;

        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : class;
        Task<int> DeleteAsync<T>(int id) where T : class;
        Task<int> DeleteAsync<T>(IEnumerable<int> id) where T : class;
        Task<int> UpdateAsync<T>(IEnumerable<T> entities) where T : class;
        Task<int> UpdateAllFieldAsync<T>(T entity) where T : class;

       IQueryable<T> IQueryableAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<T> FindEntityAsync<T>(object KeyValue) where T : class;
        Task<T> FindEntityAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<IEnumerable<T>> FindListAsync<T>() where T : class, new();
        Task<IEnumerable<T>> FindListASCAsync<T>(Expression<Func<T, object>> condition) where T : class, new();
        Task<IEnumerable<T>> FindListDESCAsync<T>(Expression<Func<T, object>> condition) where T : class, new();
        Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> condition) where T : class, new();
        Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();
        Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new();
        Task<DataTable> FindTableAsync(string strSql);
        Task<DataTable> FindTableAsync(string strSql, DbParameter[] dbParameter);
        Task<(int total, DataTable)> FindTableAsync(string strSql, string sort, bool isAsc, int pageSize, int pageIndex);
        Task<(int total, DataTable)> FindTableAsync(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex);

        Task<object> FindObjectAsync(string strSql);
        Task<object> FindObjectAsync(string strSql, DbParameter[] dbParameter);
        Task<T> FindObjectAsync<T>(string strSql) where T : class;
        #endregion
    }
}
