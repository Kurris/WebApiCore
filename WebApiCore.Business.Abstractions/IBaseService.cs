using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Model;

namespace WebApiCore.Business.Abstractions
{
    /// <summary>
    /// 基础服务
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IBaseService<T> where T : BaseEntity
    {
        #region 查询
        /// <summary>
        /// 查询一个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns><see cref="TData{T}"/></returns>
        Task<TData<T>> FindAsync(int id);

        /// <summary>
        /// 查询一个实体
        /// </summary>
        /// <param name="predicate">表达式条件</param>
        /// <returns><see cref="TData{T}"/></returns>
        Task<TData<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询多个实体
        /// </summary>
        /// <param name="predicate">表达式条件</param>
        /// <returns><see cref="TData{Enumerable{int}}"/></returns>
        Task<TData<IEnumerable<T>>> FindListAsync(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 查询多个实体
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <returns><see cref="TData{Enumerable{int}}"/></returns>
        Task<TData<IEnumerable<T>>> FindWithPagination(Pagination pagination);

        /// <summary>
        /// 查询多个实体
        /// </summary>
        /// <param name="condition">表达式条件</param>
        /// <param name="pagination">分页参数</param>
        /// <returns><see cref="TData{Enumerable{int}}"/></returns>
        Task<TData<IEnumerable<T>>> FindWithPagination(Expression<Func<T, bool>> predicate, Pagination pagination);
        #endregion

        #region 保存
        /// <summary>
        /// 保存多个实体
        /// </summary>
        /// <param name="ts">实体集合</param>
        /// <returns><see cref="TData{Enumerable{int}}"/></returns>
        Task<TData<IEnumerable<int>>> SaveAsync(IEnumerable<T> ts);
        Task<TData<int>> SaveAsync(T t);
        #endregion

        #region 删除
        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns><see cref="TData{string}"/></returns>
        Task<TData<string>> DeleteAsync(int id);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <returns><see cref="TData{string}"/></returns>
        Task<TData<string>> DeleteAsync(T t);

        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <param name="ts">实体集合</param>
        /// <returns><see cref="TData{string}"/></returns>
        Task<TData<string>> DeleteAsync(IEnumerable<T> ts);


        #endregion
    }
}
