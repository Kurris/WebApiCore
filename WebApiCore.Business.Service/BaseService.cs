using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Lib.Utils.Model;


namespace WebApiCore.Business.Service
{
    /// <summary>
    /// 基础服务
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        public virtual async Task<TData<string>> DeleteAsync(int id)
        {
            var obj = new TData<string>();
            try
            {
                await EFDB.Instance.DeleteAsync<T>(id);
                obj.Message = "删除成功";
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetInnerException();
            }
            return obj;
        }
        public virtual async Task<TData<string>> DeleteAsync(T t)
        {
            var obj = new TData<string>();
            try
            {
                await EFDB.Instance.DeleteAsync(t);
                obj.Status = Status.Success;
                obj.Message = "删除成功";
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetInnerException();
            }
            return obj;
        }



        public virtual async Task<TData<T>> FindAsync(int id)
        {
            var td = new TData<T>();
            try
            {
                var jobData = await EFDB.Create().FindAsync<T>(id);
                if (jobData == null)
                {
                    td.Status = Status.Fail;
                    td.Message = "查询失败";
                }
                else
                {
                    td.Data = jobData;
                    td.Status = Status.Success;
                    td.Message = "查询成功";
                }
            }
            catch (Exception ex)
            {
                td.Message = ex.GetInnerException();
            }
            return td;
        }
        public virtual async Task<TData<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var td = new TData<T>();
            try
            {
                var jobData = await EFDB.Instance.FindAsync(predicate);
                if (jobData == null)
                {
                    td.Status = Status.Fail;
                    td.Message = "查询失败";
                }
                else
                {
                    td.Data = jobData;
                    td.Status = Status.Success;
                    td.Message = "查询成功";
                }
            }
            catch (Exception ex)
            {
                td.Message = ex.GetInnerException();
            }
            return td;
        }

        public virtual async Task<TData<IEnumerable<T>>> FindListAsync(Expression<Func<T, bool>> predicate = null)
        {
            var td = new TData<IEnumerable<T>>();
            try
            {
                var jobData = await EFDB.Instance.FindListAsync(predicate);
                if (jobData == null)
                {
                    td.Status = Status.Fail;
                    td.Message = "查询失败";
                }
                else
                {
                    td.Data = jobData;
                    td.Status = Status.Success;
                    td.Message = "查询成功";
                }
            }
            catch (Exception ex)
            {
                td.Message = ex.GetInnerException();
            }
            return td;
        }



        public virtual async Task<TData<string>> SaveAsync(IEnumerable<T> ts)
        {
            var td = new TData<string>();
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
                td.Message = ex.GetInnerException();
            }
            return td;
        }
        public virtual async Task<TData<string>> SaveAsync(T t)
        {
            var obj = new TData<string>();
            try
            {
                var op = EFDB.Instance.GetIDataBaseOperation();
                (string key, int value) = op.FindPrimaryKeyValue(t);
                if (value == 0)
                {
                    await op.AddAsync(t);
                    obj.Message = "新增成功";
                }
                else
                {
                    await op.UpdateAsync(t);
                    obj.Message = "修改成功";
                }
                obj.Status = Status.Success;
            }
            catch (Exception ex)
            {
                obj.Message = ex.GetInnerException();
            }
            return obj;
        }
    }
}
