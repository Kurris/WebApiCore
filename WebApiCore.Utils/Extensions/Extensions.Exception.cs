using System;

namespace WebApiCore.Utils.Extensions
{
    public static partial class Extensions
    {
        public static string GetInnerException(this Exception exception)
        {
            if (exception.InnerException == null)
            {
                return exception.Message;
            }
            return exception.InnerException.GetInnerException();
        }
    }
}
