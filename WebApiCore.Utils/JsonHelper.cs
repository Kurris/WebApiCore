using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Utils
{
    public class JsonHelper
    {
        public static T ToObejct<T>(string json) => json.IsEmpty() ? default(T) : JsonConvert.DeserializeObject<T>(json);

        public static JObject ToJObject(string json) => json.IsEmpty() ? JObject.Parse("{}") : JObject.Parse(json);

        public static string ToJson(object obj) => obj.IsEmpty() ? string.Empty : JsonConvert.SerializeObject(obj);

        public static string ToJsonIgnoreLoop(object obj) =>
            obj.IsEmpty()
            ? string.Empty
            : JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
    }

    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value?.ParseToString().ParseToDateTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            DateTime? dt = value as DateTime?;
            if (dt == null)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteValue(dt.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
