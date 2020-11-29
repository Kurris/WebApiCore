using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.IdGenerator
{
    /// <summary>
    /// ID生成帮助类
    /// </summary>
    public class IdGeneratorHelper
    {

        private Snowflake snowflake;

        private IdGeneratorHelper()
        {
            snowflake = new Snowflake(2, 0, 0);
        }
        public static IdGeneratorHelper Instance { get; } = new IdGeneratorHelper();

        /// <summary>
        /// 获取分布式唯一Id
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            return snowflake.NextId();
        }
    }
}
