using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Cache;
using WebApiCore.Core;
using WebApiCore.Core.TokenHelper;
using WebApiCore.EF;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Interface;
using WebApiCore.Utils;
using WebApiCore.Utils.Model;

namespace WebApiCore.Service.SystemManager
{
    public class UserService : IUserService
    {
        public ILogger<UserService> Logger { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        public async Task<TData<User>> Login(string userName, string password)
        {
            var op = await InitDB.Create().BeginTransAsync();
            try
            {
                User user = await op.FindAsync<User>(x => x.UserName == userName);
                TData<User> obj = new TData<User>();

                if (user == null)
                {
                    obj.Message = "用户不存在";
                }
                else
                {
                    string encrypt = SecurityHelper.MD5Encrypt(password);
                    if (user.Password != encrypt)
                    {
                        obj.Message = "密码错误";
                    }
                    else
                    {
                        user.LastLogin = DateTime.Now;
                        obj.Message = "登陆成功";
                        obj.Status = Status.LoginSuccess;
                        obj.Data = user;

                        await Operator.Instance.AddCurrent(user);
                        return obj;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                Logger.LogError(ex, ex.InnerException?.Message);
                throw;
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>退出信息</returns>
        public async Task<TData<string>> LoginOff(string userName)
        {
            await Operator.Instance.RemoveCurrent(userName);
            return new TData<string>()
            {
                Message = "退出成功",
                Status = Status.Success
            };
        }

        public async Task<string> SignOut(string userName, string password)
        {
            var op = await InitDB.Create().BeginTransAsync();
            try
            {
                string encrypt = SecurityHelper.MD5Encrypt(password);
                User existUser = await op.FindAsync<User>(x => x.UserName == userName && x.Password == encrypt);
                if (existUser == null)
                {
                    return $"注销失败,用户{userName}不存在";
                }

                await op.DeleteAsync(existUser);
                return "注销成功";
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                Logger.LogError(ex, ex.InnerException?.Message);
                return "注销失败";
            }
        }

        public async Task<string> SignUp(User user)
        {
            var op = await InitDB.Create().BeginTransAsync();
            try
            {
                User existUser = await op.FindAsync<User>(x => x.UserName == user.UserName);
                if (existUser != null)
                {
                    return $"注册失败,已经存在用户{user.UserName}";
                }
                user.Password = SecurityHelper.MD5Encrypt(user.Password);
                await InitDB.Create().AddAsync(user);
                return "注册成功";
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                Logger.LogError(ex, ex.InnerException?.Message);
                return "注册失败";
            }
        }

        public async Task<User> RefreshToken()
        {
            User user = await Operator.Instance.GetCurrent();
            user.Token = JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtSetting);
            CacheFactory.Instance.SetCache(user.UserName, user);

            return user;
        }
    }
}
