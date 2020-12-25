using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCore.Business.Abstractions
{
    public interface IAutoJobService 
    {

        #region JobCenter

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> StartJob(int id);

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> EditJob(int id);

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
