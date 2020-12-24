using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.AutoJobInterface;
using WebApiCore.EF;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Extensions;
using WebApiCore.Utils.Model;

namespace WebApiCore.Service.SystemManage
{
    public class AutoJobService : IAutoJobService
    {

        public IJobCenter JobCenter { get; set; }

        public async Task<TData<string>> AddJob(AutoJobTask autoJob)
        {
            TData<string> obj = new TData<string>();
            var op = await EFDB.Create().BeginTransAsync();
            try
            {
                AutoJobTask job = await op.FindAsync<AutoJobTask>(x => x.JobName == autoJob.JobName && x.JobGroup == autoJob.JobGroup);
                if (job != null)
                {
                    obj.Message = $"任务组{autoJob.JobGroup}已存在任务名{autoJob.JobName}";
                }
                else
                {
                    await op.AddAsync(autoJob);
                    await op.CommitTransAsync();
                    await JobCenter.AddNewJob(autoJob.AutoJobTaskId);

                    obj.Message = $"任务{autoJob.JobName}添加成功";

                }
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.Message;
            }
            return obj;
        }

        public async Task<TData<string>> EditJob(AutoJobTask autoJob)
        {
            TData<string> obj = new TData<string>();
            var op = await EFDB.Create().BeginTransAsync();
            try
            {
                await op.UpdateAsync(autoJob);
                await op.CommitTransAsync();
                await JobCenter.EditJob(autoJob.AutoJobTaskId);

                obj.Message = $"任务{autoJob.JobName}修改成功";
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.GetInnerException();
            }

            return obj;
        }

        public async Task<AutoJobTask> GetAutoJob(string name, string group)
        {
            return await EFDB.Create().FindAsync<AutoJobTask>(x => x.JobName == name && x.JobGroup == group);
        }

        public async Task<IEnumerable<AutoJobTask>> GetJobList()
        {
            return await EFDB.Create().FindListAsync<AutoJobTask>();
        }

        public async Task<TData<string>> RemoveJob(int id)
        {
            var obj = new TData<string>();

            if (await JobCenter.RemoveJob(id))
            {
                await EFDB.Create().DeleteAsync<AutoJobTask>(id);

                obj.Message = "移除自动任务成功";
                obj.Status = Status.Success;
                return obj;
            }
            obj.Message = "移除失败";
            return obj;
        }

        public async Task<bool> StopAll()
        {
            return await JobCenter.StopAll();
        }
    }
}
