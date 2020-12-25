using System.Threading.Tasks;

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
        /// 任务执行入口
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns><see cref="Task"/></returns>
        public abstract Task Excute(string[] args);
    }
}
