using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Service.SystemManage
{
    public class AutoJobService : IAutoJobService
    {
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
                    return obj;
                }

                await op.AddAsync(autoJob);
                await op.CommitTransAsync();

                obj.Message = $"任务{autoJob.JobName}添加成功";
                return obj;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.Message;
                return obj;
            }
        }

        public async Task<TData<string>> EditJob(AutoJobTask autoJob)
        {
            TData<string> obj = new TData<string>();
            await EFDB.Create().UpdateAsync(autoJob);

            obj.Message = $"任务{autoJob.JobName}修改成功";
            return obj;
        }

        public async Task ExecProc(string procedure, string[] args)
        {
            await EFDB.Create().ExecProcAsync(procedure);
        }

        public async Task<IEnumerable<AutoJobTask>> GetActiveJobList()
        {
            return await EFDB.Create().FindListAsync<AutoJobTask>(x => x.JobStatus == 1);
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
            await EFDB.Create().DeleteAsync<AutoJobTask>(id);
            return new TData<string>()
            {
                Message = "移除自动任务成功",
                Status = Status.Success
            };
        }

        public Task StopAll()
        {
            return null;
        }
    }
}
