﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils.Extensions;

using WebApiCore.Lib.AutoJob.Abstractions;
using System.Linq.Expressions;

namespace WebApiCore.Business.Service
{
    public class AutoJobService : BaseService<AutoJobTask>, IAutoJobService
    {
        public IJobCenter JobCenter { get; set; }

        #region JobCenter

        public async Task<string> StartJob(int id)
        {
            try
            {
                if (await JobCenter.Start(id))
                {
                    return "任务启动成功";
                }
                else
                {
                    return "任务启动失败";
                }

            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        public async Task<string> RestartJob(int id)
        {
            try
            {
                await JobCenter.EditJob(id);
                return "任务修改完成";
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        public async Task<string> StopAll()
        {
            try
            {
                await EFDB.Instance.UpdateAsync(new List<Expression<Func<AutoJobTask, bool>>>()
                {
                    x => x.JobStatus == 0
                }, null);
                await JobCenter.StopAll();
                return "任务停止成功";
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }

        }
        public async Task<string> StopJob(int id)
        {
            try
            {
                await EFDB.Create().UpdateAsync(new List<Expression<Func<AutoJobTask, bool>>>()
                {
                    x => x.JobStatus == 0
                }, x => x.AutoJobTaskId == id);

                await JobCenter.StopJob(id);
                return "任务停止成功";
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }



        #endregion

    }
}
