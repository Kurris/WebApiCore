using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Cache;
using WebApiCore.Core.TokenHelper;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Utils;

namespace WebApiCore.Core
{
    public class Operator
    {
        private static Operator _instance = new Operator();
        public static Operator Instance { get => _instance; }

        private readonly string _loginProvider = GlobalInvariant.SystemConfig?.LoginProvider;
        private readonly string _tokenName = GlobalInvariant.SystemConfig?.JwtSetting?.TokenName;

        /// <summary>
        /// 添加当前操作
        /// </summary>
        /// <param name="user">用户信息<see cref="User"/></param>
        /// <returns>用户信息<see cref="User"/></returns>
        public async Task<User> AddCurrent(User user)
        {
            user.RefreshTime = DateTime.Now.AddMinutes(GlobalInvariant.SystemConfig.JwtSetting.Expiration);
            string jwtToken = JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtSetting);

            switch (_loginProvider)
            {
                case "WebApi":
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

            user.Token = jwtToken;

            return user;
        }

        /// <summary>
        /// 移除当前凭证
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取当前操作者
        /// </summary>
        /// <returns>User Naem<see cref="string"/></returns>
        public async Task<string> GetCurrent()
        {
            var accessor = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();

            return accessor?.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        }
    }
}