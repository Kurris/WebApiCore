using Newtonsoft.Json;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Utils.Model
{
    public class EntityErrorParam
    {
        /// <summary>
        /// 验证
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
