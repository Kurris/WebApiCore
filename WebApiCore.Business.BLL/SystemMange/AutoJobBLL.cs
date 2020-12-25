using System;
using System.Collections.Generic;
using System.Text;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.Entity.SystemManage;

namespace WebApiCore.Business.BLL.SystemMange
{
    public class AutoJobBLL
    {
        public IAutoJobService AutoJobService { get; set; }
        public IBaseService<AutoJobTask> BaseAutoJob { get; set; }
    }
}
