﻿using System.Threading.Tasks;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity.SystemManage;

namespace WebApiCore.Lib.AutoJob.Abstractions
{
    /// <summary>
    /// 任务插件抽象
    /// </summary>
    public abstract class JobPlugin
    {
        /// <summary>
        /// 当前任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前任务组名
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 获取任务当前的配置数据
        /// </summary>
        /// <returns><see cref="Task{AutoJob}"/></returns>
        protected virtual async Task<AutoJobTask> GetCurrentJobData()
        {
            return await EFDB.Instance.FindAsync<AutoJobTask>(x => x.JobName == this.Name && x.JobGroup == this.Group);
        }

        /// <summary>
        /// 任务执行入口
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns><see cref="Task"/></returns>
        public abstract Task Excute(string[] args);
    }
}