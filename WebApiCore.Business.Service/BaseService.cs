using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Model;
using Microsoft.EntityFrameworkCore;


namespace WebApiCore.Business.Service
{
    /// <summary>
    /// 基础服务
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {

        #region 查询
        public virtual async Task<TData<T>> FindAsync(int id)
        {
            var td = new TData<T>();
            try
            {
                var t = await EFDB.Instance.AsNoTracking().FindAsync<T>(id);
                if (t == null)
                {
                    td.Status = Status.Fail;
                    td.Message = "查询失败";
                }
                else
                {
                    td.Data = t;
                    td.Status = Status.Success;
                    td.Message = "查询成功";
                }
            }
            catch (Exception ex)
            {
                td.Message = ex.GetBaseException().Message;
            }
            return td;
        }
        public virtual async Task<TData<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var td = new TData<T>();
            try
            {
                var t = await EFDB.Instance.AsNoTracking().FindAsync(predicate);
                if (t == null)
                {
                    td.Status = Status.Fail;
                    td.Message = "查询失败";
                }
                else
                {
                    td.Data = t;
                    td.Status = Status.Success;
                    td.Message = "查询成功";
                }
            }
            catch (Exception ex)
            {
                td.Message = ex.GetBaseException().Message;
            }
            return td;
        }
        public virtual async Task<TData<IEnumerable<T>>> FindListAsync(Expression<Func<T, bool>> predicate = null)
        {
            var td = new TData<IEnumerable<T>>();
            try
            {
                var ts = await EFDB.Instance.AsNoTracking().FindListAsync(predicate);
                if (ts == null)
                {
                    td.Status = Status.Fail;
                    td.Message = "查询失败";
                }
                else
                {
                    td.Data = ts;
                    td.Status = Status.Success;
                    td.Message = "查询成功";
                }
            }
            catch (Exception ex)
            {
                td.Message = ex.GetBaseException().Message;
            }
            return td;
        }
        #endregion

        #region 分页查询
        public async Task<TData<IEnumerable<T>>> FindWithPagination(Pagination pagination)
        {
            var td = new TData<IEnumerable<T>>();

            try
            {
                (int total, IEnumerable<T> ts) = await EFDB.Instance.AsNoTracking().FindListAsync<T>(
                                                sortColumn: pagination.SortColumn,
                                                isAsc: pagination.IsASC,
                                                pageSize: pagination.PageSize,
                                                pageIndex: pagination.PageIndex
                                                );

                pagination.Total = total;
                td.Data = ts;
                td.Status = Status.Success;
                td.Message = "查询成功";
            }
            catch (Exception ex)
            {
                td.Message = ex.GetBaseException().Message;
            }

            return td;
        }

        public async Task<TData<IEnumerable<T>>> FindWithPagination(Expression<Func<T, bool>> predicate, Pagination pagination)
        {
            var td = new TData<IEnumerable<T>>();

            try
            {
                (int total, IEnumerable<T> ts) = await EFDB.Instance.AsNoTracking().FindListAsync(
                                                predicate: predicate,
                                                sortColumn: pagination.SortColumn,
                                                isAsc: pagination.IsASC,
                                                pageSize: pagination.PageSize,
                                                pageIndex: pagination.PageIndex
                                                );

                pagination.Total = total;
                td.Data = ts;
                td.Status = Status.Success;
                td.Message = "查询成功";
            }
            catch (Exception ex)
            {
                td.Message = ex.GetBaseException().Message;
            }

            return td;
        }
        #endregion

        #region 删除
        public virtual async Task<TData<string>> DeleteAsync(int id)
        {
            var obj = new TData<string>();
            try
            {
                await EFDB.Instance.AsNoTracking().DeleteAsync<T>(id);
                obj.Message = "删除成功";
                obj.Status = Status.Success;
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetBaseException().Message;
            }
            return obj;
        }
        public virtual async Task<TData<string>> DeleteAsync(T t)
        {
            var obj = new TData<string>();
            try
            {
                await EFDB.Instance.AsNoTracking().DeleteAsync(t);
                obj.Status = Status.Success;
                obj.Message = "删除成功";
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetBaseException().Message;
            }
            return obj;
        }
        public async Task<TData<string>> DeleteAsync(IEnumerable<T> ts)
        {
            var obj = new TData<string>();

            try
            {
                await EFDB.Instance.AsNoTracking().DeleteAsync(ts);
                obj.Status = Status.Success;
                obj.Message = "删除成功";
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetBaseException().Message;
            }
            return obj;
        }
        #endregion

        #region 保存
        public virtual async Task<TData<IEnumerable<int>>> SaveAsync(IEnumerable<T> ts)
        {
            var td = new TData<IEnumerable<int>>();
            var op = await EFDB.Instance.BeginTransAsync();

            try
            {
                foreach (var t in ts)
                {
                    (string key, int value) = op.FindPrimaryKeyValue(t);
                    if (value == 0)
                    {
                        await op.AddAsync(t);
                    }
                    else
                    {
                        await op.UpdateAsync(t);
                    }
                }

                await op.CommitTransAsync();
                td.Message = "保存成功";
                td.Status = Status.Success;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                td.Message = ex.GetBaseException().Message;
            }
            return td;
        }
        public virtual async Task<TData<int>> SaveAsync(T t)
        {
            var obj = new TData<int>();
            var op = await EFDB.Instance.BeginTransAsync();
            try
            {
                (string key, int value) = op.FindPrimaryKeyValue(t);
                if (value == 0)
                {
                    await op.AddAsync(t);
                }
                else
                {
                    await op.UpdateAsync(t);
                }
                obj.Message = "保存成功";
                obj.Status = Status.Success;
                obj.Data = value;

                await op.CommitTransAsync();
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetBaseException().Message;
                await op.RollbackTransAsync();
            }
            return obj;
        }
        #endregion
    }
}
