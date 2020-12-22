using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace WebApiCore.Utils.Extensions
{
    public static class Serialization
    {
        /// <summary>
        /// 影子克隆
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">实例</param>
        /// <returns><see cref="{T}"/></returns>
        public static T ShadowClone<T>(this T t) where T : ICloneable, new()
        {
            return (T)t.Clone();
        }

        /// <summary>
        /// 深克隆
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">实例</param>
        /// <returns><see cref="{T}"/></returns>
        public static T DeepCloneByBinary<T>(this T t) where T : class, new()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, t);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// 深克隆
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">实例</param>
        /// <returns><see cref="{T}"/></returns>
        public static T DeepCloneByXML<T>(this T t) where T : class, new()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, t);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)xml.Deserialize(ms);
            }
        }
    }
}
