using System.ComponentModel;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Utils.Model
{
    /// <summary>
    /// 数据结果返回模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class TData<T>
    {
        /// <summary>
        /// 数据结果返回模型
        /// </summary>
        public TData()
        {

        }

        /// <summary>
        /// 数据结果返回模型
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">数据</param>
        /// <param name="status">状态</param>
        public TData(string message, T data, Status status)
        {
            this.Message = message;
            this.Data = data;
            this.Status = status;
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结果内容
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 状态(默认:Fail)
        /// </summary>
        public Status Status { get; set; } = Status.Fail;
    }

    /// <summary>
    /// 返回状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 0,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error = 3,

        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限")]
        NoPermission = 4,

        /// <summary>
        /// 鉴权失败
        /// </summary>
        [Description("鉴权失败")]
        AuthorizationFail = 5,

        /// <summary>
        /// 实体验证失败
        /// </summary>
        [Description("实体验证失败")]
        ValidateEntityError = 6,

        /// <summary>
        /// 登录成功
        /// </summary>
        [Description("登录成功")]
        LoginSuccess = 7
    }
}
