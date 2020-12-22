using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Interface.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Service.SystemManage
{
    public class AutoJobService : IAutoJobService
    {
        public async Task<TData<string>> AddJob(AutoJob autoJob)
        {
            TData<string> obj = new TData<string>();
            var op = await EFDB.Create().BeginTransAsync();
            try
            {
                AutoJob job = await op.FindAsync<AutoJob>(x => x.Name == autoJob.Name && x.Group == autoJob.Group);
                if (job != null)
                {
                    obj.Message = $"任务组{autoJob.Group}已存在任务名{autoJob.Name}";
                    return obj;
                }

                await op.AddAsync(autoJob);
                await op.CommitTransAsync();

                obj.Message = $"任务{autoJob.Name}添加成功";
                return obj;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.Message;
                return obj;
            }
        }

        public async Task<TData<string>> EditJob(AutoJob autoJob)
        {
            TData<string> obj = new TData<string>();
            await EFDB.Create().UpdateAsync(autoJob);

            obj.Message = $"任务{autoJob.Name}修改成功";
            return obj;
        }

        public async Task ExecProc(string procedure, string[] args)
        {
            await EFDB.Create().ExecProcAsync(procedure);
        }

        public async Task<IEnumerable<AutoJob>> GetActiveJobList()
        {
            return await EFDB.Create().FindListAsync<AutoJob>(x => x.Status == 1);
        }

        public async Task<AutoJob> GetAutoJob(string name, string group)
        {
            return await EFDB.Create().FindAsync<AutoJob>(x => x.Name == name && x.Group == group);
        }

        public async Task<IEnumerable<AutoJob>> GetJobList()
        {
            return await EFDB.Create().FindListAsync<AutoJob>();
        }

        public async Task<TData<string>> RemoveJob(int id)
        {
            await EFDB.Create().DeleteAsync<AutoJob>(id);
            return new TData<string>()
            {
                Message = "移除自动任务成功",
                Status = Status.Success
            };
        }
    }
}
