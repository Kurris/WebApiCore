using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiCore.Data.Entity;

namespace WebApiCore.Data.EF.DataBase
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public interface IDataBaseOperation : IAsyncDisposable, IDisposable
    {

        /// <summary>
        /// 数据库当前上下文
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// 数据库当前上下文事务
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; }


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


        /// <summary>
        /// 查找主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实例</param>
        /// <returns>主键和值<see cref="(string key, int value)"/></returns>
        (string key, int value) FindPrimaryKeyValue<T>(T t) where T : BaseEntity;

        /// <summary>
        /// 查找主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>主键名称<see cref="string"/></returns>
        string FindPrimaryKey<T>() where T : BaseEntity;

        /// <summary>
        /// 转成IQueryable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">表达式</param>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IQueryable<T> AsQueryable<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;


        /// <summary>
        /// 转成IQueryable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IQueryable<T> AsQueryable<T>() where T : BaseEntity;

        /// <summary>
        /// 转成无跟踪
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns><see cref="IQueryable{T}"/></returns>
        IQueryable<T> AsNoTracking<T>() where T : BaseEntity;

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">sql字符串</param>
        /// <param name="keyValues">参数</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> RunSqlAsync(string strSql, IDictionary<string, object> keyValues = null);

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">内插sql字符串</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> RunSqlInterAsync(FormattableString strSql);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="keyValues">参数</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> ExecProcAsync(string procName, IDictionary<string, object> keyValues = null);


        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> AddAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 添加一组实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">一组实例</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> AddAsync<T>(IEnumerable<T> entities) where T : BaseEntity;

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实例</param>
        /// <param name="updateAll">全部更新</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> UpdateAsync<T>(T entity, bool updateAll = false) where T : BaseEntity;

        /// <summary>
        /// 更新一组实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">一组实例</param>
        /// <param name="updateAll">全部更新</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> UpdateAsync<T>(IEnumerable<T> entities, bool updateAll = false) where T : BaseEntity;

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="wherePredicate">Where表达式/param>
        /// <param name="setPredicates">SQL中的SET赋值表达式</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> UpdateAsync<T>(Expression<Func<T, bool>> wherePredicate, params Expression<Func<T, bool>>[] setPredicates) where T : BaseEntity;

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实例</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> DeleteAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 删除一组实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">一组实例</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> DeleteAsync<T>(IEnumerable<T> entities) where T : BaseEntity;

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">主键</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> DeleteAsync<T>(int keyValue) where T : BaseEntity;

        /// <summary>
        /// 删除一组实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValues">一组主键</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> DeleteAsync<T>(IEnumerable<int> keyValues) where T : BaseEntity;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="propName">字段</param>
        /// <param name="propValue">值</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> DeleteAsync<T>(string propName, object propValue) where T : BaseEntity;


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">where表达式</param>
        /// <returns>返回受影响行<see cref="int"/></returns>
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="KeyValue">主键</param>
        /// <returns>实体<see cref="{T}"/></returns>
        Task<T> FindAsync<T>(params object[] keyValues) where T : BaseEntity;

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">表达式</param>
        /// <returns>实体<see cref="{T}"/></returns>
        Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;

        /// <summary>
        /// 返回一组数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">表达式</param>
        /// <returns>所有数据<see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<T>> FindListAsync<T>(Expression<Func<T, bool>> predicate = null) where T : BaseEntity;

        /// <summary>
        /// 排序查询一组数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">表达式</param>
        /// <returns>一组数据<see cref="IEnumerable{T}"/></returns>
        Task<IEnumerable<T>> FindListByOrderAsync<T>(Expression<Func<T, object>> predicate, bool isAsc) where T : BaseEntity;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sortColumn">排序列</param>
        /// <param name="isAsc">ASC排序</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns>总数<see cref="int"/> 当前页<see cref="IEnumerable{T}"/></returns>
        Task<(int total, IEnumerable<T> list)> FindListAsync<T>(string sortColumn, bool isAsc, int pageSize,
                                                                int pageIndex) where T : BaseEntity;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">表达式</param>
        /// <param name="sortColumn">排序列</param>
        /// <param name="isAsc">ASC排序</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns>总数<see cref="int"/> 当前页<see cref="IEnumerable{T}"/></returns>
        Task<(int total, IEnumerable<T> list)> FindListAsync<T>(Expression<Func<T, bool>> predicate, string sortColumn, bool isAsc,
                                                                int pageSize, int pageIndex) where T : BaseEntity;


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="strSql">数据源</param>
        /// <param name="dbParameters">参数</param>
        /// <param name="sortColumn">排序列</param>
        /// <param name="isAsc">ASC排序</param>
        /// <param name="pageSize">页行数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns>总数<see cref="int"/> 当前页<see cref="DataTable"/></returns>
        Task<(int total, DataTable)> FindTableAsync(string strSql, IDictionary<string, object> dbParameters, string sortColumn, bool isAsc, int pageSize, int pageIndex);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="strSql">sql字符串</param>
        /// <param name="keyValues">参数</param>
        /// <returns><see cref="DataTable"/></returns>
        Task<DataTable> GetTableAsync(string strSql, IDictionary<string, object> keyValues = null);


        /// <summary>
        /// 获取首行数据
        /// </summary>
        /// <param name="strSql">sql字符串</param>
        /// <param name="keyValues">参数</param>
        /// <returns>首行<see cref="IDataReader"/></returns>
        Task<IDataReader> GetReaderAsync(string strSql, IDictionary<string, object> keyValues = null);

        /// <summary>
        /// 获取首行首列值
        /// </summary>
        /// <param name="strSql">sql字符串</param>
        /// <param name="keyValues">参数</param>
        /// <returns>首行首列<see cref="object"/></returns>
        Task<object> GetScalarAsync(string strSql, IDictionary<string, object> keyValues = null);
    }
}
