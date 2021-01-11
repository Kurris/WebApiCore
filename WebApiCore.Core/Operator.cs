using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApiCore.Core.TokenHelper;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Core
{
    public class Operator
    {
        private static readonly Operator _instance = new Operator();
        public static Operator Instance { get => _instance; }

        private readonly string _loginProvider = GlobalInvariant.SystemConfig?.LoginProvider;
        private readonly string _tokenName = GlobalInvariant.SystemConfig?.JwtConfig?.TokenName;

        /// <summary>
        /// 添加当前操作
        /// </summary>
        /// <param name="user">用户信息<see cref="User"/></param>
        /// <returns>用户信息<see cref="User"/></returns>
        public async Task<User> AddCurrent(User user)
        {
            user.RefreshTime = DateTime.Now.AddMinutes(GlobalInvariant.SystemConfig.JwtConfig.Expiration);
            string jwtToken = JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtConfig);

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

            return await Task.FromResult(user);
        }

        /// <summary>
        /// 移除当前凭证
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RemoveCurrent()
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

            return await Task.FromResult(true);
        }

        /// <summary>
        /// 获取当前操作者
        /// </summary>
        /// <returns>User Naem<see cref="string"/></returns>
        public async Task<string> GetCurrent()
        {
            var accessor = GlobalInvariant.ServiceProvider?.GetService<IHttpContextAccessor>();
            return await Task.FromResult(accessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "name")?.Value);
        }
    }
}