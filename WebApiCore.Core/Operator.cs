using System;
using System.Threading.Tasks;
using WebApiCore.Cache;
using WebApiCore.Core.TokenHelper;
using WebApiCore.Entity;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Utils;

namespace WebApiCore.Core
{
    public class Operator
    {
        private static Operator _instance = new Operator();
        public static Operator Instance { get => _instance; }

        private readonly static string _tokenKey = GlobalInvariant.SystemConfig.JwtSetting.TokenName;

        /// <summary>
        /// 添加当前操作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> AddCurrent(User user)
        {
            string loginProvider = GlobalInvariant.SystemConfig.LoginProvider;
            string jwtToken = JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtSetting);

            switch (loginProvider)
            {
                case "WebApi":
                    user.Token = jwtToken;
                    break;
                case "Cookie":
                    new CookieHelper().AddCookie(_tokenKey, jwtToken);
                    break;
                case "Session":
                    new SessionHelper().AddSession(_tokenKey, jwtToken);
                    break;
                default:
                    throw new NotSupportedException(loginProvider);
            }

            CacheFactory.Instance.SetCache(user.UserName, user, DateTime.Now.AddMinutes(2));

            return user;
        }
    }
}