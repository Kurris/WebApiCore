using Newtonsoft.Json;

namespace Ligy.Project.WebApi.CustomClass
{
    public class ValidationError
    {
        /// <summary>
        /// 字段
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        /// <summary>
        /// 提醒内容
        /// </summary>
        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = !string.IsNullOrEmpty(field) ? field : string.Empty;
            Message = message;
        }
    }
}
