using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Data.Entity.BlogInfos;
using WebApiCore.Lib.Utils.Model;

namespace WebApiCore.Business.Abstractions.SystemManage
{
    public interface IProfileService :IBaseService<Profile>
    {
        Task<TData<Profile>> GetProfile(string name);
    }
}
