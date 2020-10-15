namespace Ligy.Project.WebApi.CustomClass
{
    /// <summary>
    /// 结果实体
    /// </summary>
    public class ResultModel
    {
        public ResultModel(int? code = null,
                          string message = null,
                          object result = null,
                          ReturnStatus returnStatus = ReturnStatus.Success)
        {
            this.Code = code;
            this.Result = result;
            this.Message = message;
            this.ReturnStatus = returnStatus;
        }

        /// <summary>
        /// 代号
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结果内容
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ReturnStatus ReturnStatus { get; set; }
    }

    public enum ReturnStatus
    {
        Success = 1,
        Fail = 0,
        ConfirmIsContinue = 2,
        Error = 3
    }
}
