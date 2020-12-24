using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Entity.SystemManage
{
    [Table("AutoJobTasks")]
    public class AutoJobTask : BaseEntity
    {
        public int AutoJobTaskId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public JobExecuteType? JobType { get; set; }

        /// <summary>
        /// 执行名称
        /// </summary>
        public string ExcuteName { get; set; }

        /// <summary>
        /// 参数,以逗号分隔
        /// </summary>
        public string ExcuteArgs { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? JobStatus { get; set; }

        /// <summary>
        /// 任务表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 下次运行时间
        /// </summary>
        public DateTime? NextStartTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 任务执行类型
    /// </summary>
    public enum JobExecuteType
    {
        /// <summary>
        /// 预定
        /// </summary>
        Destine = 0,

        /// <summary>
        /// DLL插件
        /// </summary>
        DLLPlugin = 1,

        /// <summary>
        /// 存储过程
        /// </summary>
        Procedure = 2,

        /// <summary>
        /// SQL语句
        /// </summary>
        Sql = 3,
    }
}
