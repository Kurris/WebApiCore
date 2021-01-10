using System.Security.Cryptography;
using System.Text;

namespace WebApiCore.Lib.Utils
{
    /// <summary>
    /// 安全帮助类
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="bit">位数，一般取值16 或 32</param>
        /// <returns>MD5字符串</returns>
        public static string MD5Encrypt(string str, int bit = 32)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                sb.Append(i.ToString("x2"));
            }
            if (bit == 16)
            {
                return sb.ToString().Substring(8, 16).ToLower();
            }
            else
            {
                return sb.ToString().ToLower();
            }
        }
    }
}
