using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Core;
using WebApiCore.Core.TokenHelper;
using WebApiCore.Data.EF;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Model;
using WebApiCore.Data.Entity;
using System.Linq.Expressions;

namespace WebApiCore.Business.Service
{
    public class UserService : BaseService<User>, IUserService
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户信息</returns>
        public async Task<TData<User>> Login(string userName, string password)
        {
            var op = await EFDB.Instance.AsNoTracking().BeginTransAsync();
            TData<User> obj = new TData<User>();

            try
            {
                User user = await op.FindAsync<User>(x => x.UserName == userName);
                if (user == null)
                {
                    obj.Message = "用户不存在";
                    obj.Status = Status.Fail;
                }
                else
                {
                    string encrypt = SecurityHelper.MD5Encrypt(password);
                    if (user.Password != encrypt)
                    {
                        obj.Message = "密码错误";
                        obj.Status = Status.Fail;
                    }
                    else
                    {
                        DateTime? dtNow = DateTime.Now;

                        await op.UpdateAsync(new List<Expression<Func<User, bool>>>()
                        {
                            x=>x.LastLogin==dtNow
                        }, x => x.UserName == userName);

                        user.LastLogin = DateTime.Now;
                        obj.Message = "登陆成功";
                        obj.Status = Status.LoginSuccess;
                        obj.Data = user;

                        await Operator.Instance.AddCurrent(user);
                    }
                }
                await op.CommitTransAsync();
            }
            catch (Exception ex)
            {
                await op.RollbackTransAsync();
                obj.Message = ex.GetBaseException().Message;
            }
            return obj;
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
                obj.Message = ex.GetBaseException().Message;
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
                obj.Message = ex.GetBaseException().Message;
                obj.Status = Status.Error;
                return obj;
            }
        }

        public async Task<string> RefreshToken()
        {
            string userName = await Operator.Instance.GetCurrent();
            User user = await EFDB.Create().FindAsync<User>(x => x.UserName == userName);
            return JwtHelper.GenerateToken(user, GlobalInvariant.SystemConfig.JwtConfig);
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
                obj.Message = ex.GetBaseException().Message;
                obj.Status = Status.Error;
                return obj;
            }

            obj.Data = await EFDB.Create().FindAsync<User>(user.UserId);
            return obj;
        }
    }
}
