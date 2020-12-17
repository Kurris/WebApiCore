using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManager;
using WebApiCore.Utils.Model;

namespace WebApiCore.Interface
{
    public interface IUserService
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<TData<User>> Login(string userName, string password);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> SignUp(User user);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<TData<string>> LoginOff(string userName);

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        Task<User> RefreshToken();
    }
}
