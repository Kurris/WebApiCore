using System;
using System.Threading.Tasks;

using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity.SystemManage;

using WebApiCore.Lib.AutoJob.Abstractions;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Business.Service.SystemManage
{
    public class AutoJobService : BaseService<AutoJobTask>, IAutoJobService
    {
        public IJobCenter JobCenter { get; set; }

        #region JobCenter

        public async Task<string> StartJob(int id)
        {
            try
            {
                await JobCenter.Start(id);
                return "任务启动成功";
            }
            catch (Exception ex)
            {
                return ex.GetInnerException();
            }
        }
        public async Task<string> EditJob(int id)
        {
            try
            {
                await JobCenter.EditJob(id);
                return "任务修改完成";
            }
            catch (Exception ex)
            {
                return ex.GetInnerException();
            }
        }
        public async Task<string> StopAll()
        {
            try
            {
                await EFDB.Instance.UpdateAsync<AutoJobTask>(null, x => x.JobStatus == 0);
                await JobCenter.StopAll();
                return "任务停止成功";
            }
            catch (Exception ex)
            {
                return ex.GetInnerException();
            }
        }
        public async Task<string> StopJob(int id)
        {
            try
            {
                await EFDB.Create().UpdateAsync<AutoJobTask>(x => x.AutoJobTaskId == id, x => x.JobStatus == 0);
                await JobCenter.StopJob(id);
                return "任务停止成功";
            }
            catch (Exception ex)
            {
                return ex.GetInnerException();
            }
        }



        #endregion

    }
}
