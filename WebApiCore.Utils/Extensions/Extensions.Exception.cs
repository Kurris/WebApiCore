using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return exception.GetInnerException();
        }
    }
}
