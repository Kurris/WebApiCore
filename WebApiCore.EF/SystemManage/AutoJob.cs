using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Entity.SystemManage
{
    [Table("AutoJobs")]
    public class AutoJob : BaseEntity
    {
        public int AutoJobId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string Group { get; set; }

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
        public string Paramenters { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 任务表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 任务执行间隔分钟
        /// </summary>
        public int? Minute { get; set; }

        /// <summary>
        /// 任务执行间隔秒数
        /// </summary>
        public int? Second { get; set; }

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
        /// 存储过程
        /// </summary>
        Procedure = 2,

        /// <summary>
        /// DLL插件
        /// </summary>
        DLLPlugin = 1,
    }
}
