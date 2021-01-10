using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace WebApiCore.Lib.Utils
{
    public class JsonHelper
    {
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(object obj)
        {
            if (obj == null) return string.Empty;
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToJson(object obj, JsonSetting jsonSetting)
        {
            if (obj == null) return string.Empty;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = jsonSetting.ContractResolver switch
                {
                    ContractResolver.CamelCase => new CamelCasePropertyNamesContractResolver(),
                    _ => null,
                },
                ReferenceLoopHandling = (ReferenceLoopHandling)jsonSetting.LoopHandling
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">字符串</param>
        /// <returns><see cref="{T}"/></returns>
        public static T ToObejct<T>(string jsonStr) => string.IsNullOrEmpty(jsonStr) ? default(T) : JsonConvert.DeserializeObject<T>(jsonStr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns><see cref="JObject"/></returns>
        public static JObject ToJObject(string jsonStr) => string.IsNullOrEmpty(jsonStr) ? JObject.Parse("{}") : JObject.Parse(jsonStr);

    }

    public class JsonSetting
    {
        public LoopHandling LoopHandling { get; set; }
        public ContractResolver? ContractResolver { get; set; }
    }

    public enum ContractResolver
    {
        CamelCase = 0
    }

    public enum LoopHandling
    {

        Error = 0,

        Ignore = 1,

        Serialize = 2
    }

}

