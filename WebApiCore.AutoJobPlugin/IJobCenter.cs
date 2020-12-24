using System.Threading.Tasks;

namespace WebApiCore.AutoJobInterface
{
    /// <summary>
    /// 任务中心
    /// </summary>
    public interface IJobCenter
    {

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <returns>开始结果</returns>
        Task<bool> Start();

        /// <summary>
        /// 停止所有任务
        /// </summary>
        /// <returns>停止结果</returns>
        Task<bool> StopAll();

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns>修改结果</returns>
        Task<bool> EditJob(int id);

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns>移除结果</returns>
        Task<bool> RemoveJob(int id);

        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        Task<bool> AddNewJob(int id);
    }
}
