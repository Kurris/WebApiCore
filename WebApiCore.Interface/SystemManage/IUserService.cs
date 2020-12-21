using System.Threading.Tasks;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Interface.SystemManage
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
        Task<TData<string>> SignUp(User user);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<TData<string>> LoginOff(string userName);

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns>Token</returns>
        Task<string> RefreshToken();

        /// <summary>
        /// 编辑用户资料
        /// </summary>
        /// <param name="user">需要修改的用户资料</param>
        /// <returns>用户资料</returns>
        Task<TData<User>> EditUser(User user);
    }
}
