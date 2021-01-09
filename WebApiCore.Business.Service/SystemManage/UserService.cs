using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Core;
using WebApiCore.Core.TokenHelper;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity.SystemManage;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Utils.Extensions;
using WebApiCore.Lib.Utils.Model;


namespace WebApiCore.Business.Service.SystemManage
{
    public class UserService : IUserService
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        public async Task<TData<User>> Login(string userName, string password)
        {
            var op = await EFDB.Create().BeginTransAsync();
            TData<User> obj = new TData<User>();
            obj.Status = Status.Fail;
            try
            {
                User user = await op.FindAsync<User>(x => x.UserName == userName);
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

                        await op.RunSqlAsync("UPDATE Users SET lastlogin=@lastlogin", new Dictionary<string, object>() { ["lastlogin"] = DateTime.Now });

                        user.LastLogin = DateTime.Now;
                        obj.Message = "登陆成功";
                        obj.Status = Status.LoginSuccess;
                        obj.Data = user;

                        await Operator.Instance.AddCurrent(user);
                    }
                }
                await op.CommitTransAsync();
                return obj;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Status = Status.Error;
                obj.Message = ex.GetInnerException();
                return obj;
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>退出信息</returns>
        public async Task<TData<string>> LoginOff()
        {
            await Operator.Instance.RemoveCurrent();
            return new TData<string>()
            {
                Message = "退出成功",
                Status = Status.Success
            };
        }

        public async Task<TData<string>> SignOut(string userName, string password)
        {
            var op = await EFDB.Instance.BeginTransAsync();
            TData<string> obj = new TData<string>();
            try
            {
                string encrypt = SecurityHelper.MD5Encrypt(password);
                User existUser = await op.FindAsync<User>(x => x.UserName == userName && x.Password == encrypt);
                if (existUser == null)
                {
                    obj.Message = $"注销失败,用户{userName}不存在";
                    return obj;
                }

                await op.DeleteAsync(existUser);
                await op.CommitTransAsync();

                obj.Message = "注销成功";
                obj.Status = Status.Success;
                return obj;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.GetInnerException();
                return obj;
            }
        }

        public async Task<TData<string>> SignUp(User user)
        {
            var op = await EFDB.Create().BeginTransAsync();
            TData<string> obj = new TData<string>();
            try
            {
                User existUser = await op.FindAsync<User>(x => x.UserName == user.UserName);
                if (existUser != null)
                {
                    obj.Message = $"注册失败,已经存在用户{user.UserName}";
                    return obj;
                }

                user.Password = SecurityHelper.MD5Encrypt(user.Password);
                await op.AddAsync(user);
                await op.CommitTransAsync();

                obj.Message = "注册成功";
                obj.Status = Status.Success;
                return obj;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.GetInnerException();
                obj.Status = Status.Error;
                return obj;
            }
        }

        public async Task<string> RefreshToken()
        {
            string userName = await Operator.Instance.GetCurrent();
            User user = await EFDB.Create().FindAsync<User>(x => x.UserName == userName);
            return JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtSetting);
        }

        public async Task<TData<User>> EditUser(User user)
        {
            var op = await EFDB.Create().BeginTransAsync();
            TData<User> obj = new TData<User>();
            try
            {
                await op.UpdateAsync(user);
                await op.CommitTransAsync();
                obj.Message = "修改成功";
                obj.Status = Status.Success;
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.GetInnerException();
                obj.Status = Status.Error;
                return obj;
            }

            obj.Data = await EFDB.Create().FindAsync<User>(user.UserId);
            return obj;
        }
    }
}
