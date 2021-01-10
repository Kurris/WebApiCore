using System.Threading.Tasks;
using WebApiCore.Data.Entity.SystemManage;

namespace WebApiCore.Business.Abstractions
{
    public interface IAutoJobService : IBaseService<AutoJobTask>
    {

        #region JobCenter

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> StartJob(int id);

        /// <summary>
        /// 重启任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> RestartJob(int id);

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> StopJob(int id);

        /// <summary>
        /// 停止所有任务
        /// </summary>
        /// <returns></returns>
        Task<string> StopAll();

        #endregion
    }
}
