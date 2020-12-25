using Newtonsoft.Json;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Lib.Utils.Model
{
    /// <summary>
    /// 实体验证错误
    /// </summary>
    public class EntityErrorParam
    {
        /// <summary>
        /// 实体验证错误
        /// </summary>
        /// <param name="field"></param>
        /// <param name="message"></param>
        public EntityErrorParam(string field, string message)
        {
            Field = field.IsEmpty() ? string.Empty : field;
            Message = message;
        }

        /// <summary>
        /// 字段
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        /// <summary>
        /// 提醒内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }
    }
}
