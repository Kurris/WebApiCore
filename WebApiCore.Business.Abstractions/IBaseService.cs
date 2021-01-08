using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Business.Abstractions
{
    /// <summary>
    /// 基础服务
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<TData<T>> FindAsync(int id);
        Task<TData<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<TData<IEnumerable<T>>> FindListAsync(Expression<Func<T, bool>> predicate = null);
        Task<TData<string>> SaveAsync(IEnumerable<T> ts);
        Task<TData<string>> SaveAsync(T t);
        Task<TData<string>> DeleteAsync(int id);
        Task<TData<string>> DeleteAsync(T t);
        IQueryable<T> AsQueryable();
        DbContext DbContext { get; }
    }
}
