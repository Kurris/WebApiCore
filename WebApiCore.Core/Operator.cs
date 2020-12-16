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

        private readonly string _loginProvider = GlobalInvariant.SystemConfig.LoginProvider;
        private readonly string _tokenName = GlobalInvariant.SystemConfig.JwtSetting.TokenName;

        /// <summary>
        /// 添加当前操作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> AddCurrent(User user)
        {

            string jwtToken = JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtSetting);

            switch (_loginProvider)
            {
                case "WebApi":
                    user.Token = jwtToken;
                    break;
                case "Cookie":
                    new CookieHelper().AddCookie(_tokenName, jwtToken);
                    break;
                case "Session":
                    new SessionHelper().AddSession(_tokenName, jwtToken);
                    break;
                default:
                    throw new NotSupportedException(_loginProvider);
            }

            CacheFactory.Instance.SetCache(user.UserName, user, DateTime.Now.AddMinutes(2));

            return user;
        }

        public async Task<bool> RemoveCurrent(string userName)
        {
            switch (_loginProvider)
            {
                case "WebApi":
                    break;
                case "Cookie":
                    new CookieHelper().RemoveCookie(_tokenName);
                    break;
                case "Session":
                    new SessionHelper().RemoveSession(_tokenName);
                    break;
                default:
                    throw new NotSupportedException(_loginProvider);
            }

            return CacheFactory.Instance.RemoveCache(userName);
        }
    }
}