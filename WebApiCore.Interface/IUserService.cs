using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManager;

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
        Task<string> Login(string userName, string password);

        /// <summary>
        /// 检查登录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User> CheckLogin(string userName);

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
        Task<string> LoginOff(string userName);
    }
}
