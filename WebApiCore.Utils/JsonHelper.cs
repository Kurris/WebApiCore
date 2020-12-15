using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Utils
{
    public class JsonHelper
    {
        public static T ToObejct<T>(string json) => json.IsEmpty() ? default(T) : JsonConvert.DeserializeObject<T>(json);

        public static JObject ToJObject(string json) => json.IsEmpty() ? JObject.Parse("{}") : JObject.Parse(json);

        public static string ToJson(object obj) => obj.IsEmpty() ? string.Empty : JsonConvert.SerializeObject(obj);
    }
}
