using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Utils;

namespace WebApiCore.Service.SystemManager
{
    public class OSInfoService
    {
        /// <summary>
        /// 获取OS运行信息
        /// </summary>
        /// <returns>信息<see cref="OSInfo"/></returns>
        public async Task<OSInfo> GetOSInfo()
        {
            return await OSHelper.GetOSInfo();
        }
    }
}
