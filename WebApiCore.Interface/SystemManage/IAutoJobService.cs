using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Interface.SystemManage
{
    public interface IAutoJobService
    {
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AutoJobTask>> GetJobList();

        /// <summary>
        /// 获取匹配的任务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        Task<AutoJobTask> GetAutoJob(string name, string group);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="autoJob"></param>
        /// <returns></returns>
        Task<TData<string>> AddJob(AutoJobTask autoJob);

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="autoJob"></param>
        /// <returns></returns>
        Task<TData<string>> EditJob(AutoJobTask autoJob);

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TData<string>> RemoveJob(int id);

        /// <summary>
        /// 停止所有任务
        /// </summary>
        /// <returns></returns>
        Task<bool> StopAll();
    }
}
