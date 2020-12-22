using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.EF;
using WebApiCore.Entity.SystemManage;

namespace WebApiCore.AutoJobInterface
{
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
        protected virtual async Task<AutoJob> GetJobData()
        {
            return await EFDB.Create().FindAsync<AutoJob>(x => x.Name == this.Name && x.Group == this.Group);
        }

        public abstract Task Excute(string[] args);
    }
}
