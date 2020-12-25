using System.Threading.Tasks;

namespace WebApiCore.Lib.AutoJob.Abstractions
{
    /// <summary>
    /// 任务中心接口
    /// </summary>
    public interface IJobCenter
    {

        /// <summary>
        /// 获取所有启用的任务,并且开启
        /// </summary>
        /// <returns>启动结果<see cref="bool"/></returns>
        Task<bool> Start();

        /// <summary>
        /// 停止当前运行的任务
        /// </summary>
        /// <returns>停止结果<see cref="bool"/></returns>
        Task<bool> StopAll();

        /// <summary>
        /// 启动一个任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns>启动结果<see cref="bool"/></returns>
        Task<bool> Start(int id);

        /// <summary>
        /// 修改任务(重新获取数据,并开启任务)
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns>修改结果<see cref="bool"/></returns>
        Task<bool> EditJob(int id);

        /// <summary>
        /// 任务暂停
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns>暂停结果<see cref="bool"/></returns>
        Task<bool> StopJob(int id);

        /// <summary>
        /// 检查任务存在
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="group">任务组名</param>
        /// <returns>存在结果<see cref="bool"/></returns>
        Task<bool> Exists(string name, string group);

    }
}
