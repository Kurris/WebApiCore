using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Utils.Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PaginationParam
    {

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; } = 10;//默认10行数据

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 排序列
        /// </summary>
        public string SortColumn { get; set; } = "Id"; //默认Id为排序列

        /// <summary>
        /// 排序类型
        /// </summary>
        public bool IsASC { get; set; } = false;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (TotalCount > 0)
                {
                    return (TotalCount % PageSize) == 0
                                            ? TotalCount / PageSize
                                            : TotalCount / PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
