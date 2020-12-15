using System.ComponentModel;
using WebApiCore.Utils.Extensions;

namespace WebApiCore.Utils.Model
{
    /// <summary>
    /// 数据结果返回模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TData<T>
    {
        /// <summary>
        /// 数据结果返回模型
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="data">数据</param>
        /// <param name="returnStatus">状态</param>
        public TData(string message, T data, ReturnStatus returnStatus)
        {
            this.Message = message;
            this.Data = data;
            this.ReturnStatus = returnStatus;
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
        /// 状态
        /// </summary>
        public ReturnStatus ReturnStatus { get; set; }
    }

    /// <summary>
    /// 返回状态
    /// </summary>
    public enum ReturnStatus
    {
        [Description("失败")]
        Fail = 0,

        [Description("成功")]
        Success = 1,

        [Description("错误")]
        Error = 3,

        [Description("无权限")]
        NoPermission = 4,

        [Description("实体验证失败")]
        ValidateEntityError = 5,
    }
}
