using System;
using System.Threading.Tasks;
using WebApiCore.Business.Abstractions;
using WebApiCore.Data.EF;
using WebApiCore.Data.Entity;
using WebApiCore.Lib.Model;

namespace WebApiCore.Business.Service
{
    public class ProfileService : BaseService<Profile>, IProfileService
    {
        public async Task<TData<Profile>> GetProfile(string name)
        {
            var td = new TData<Profile>();
            try
            {
                td.Data = await EFDB.Instance.FindAsync<Profile>(x => x.Name == name);
                td.Status = Status.Success;
            }
            catch (Exception ex)
            {
                td.Message = ex.GetBaseException().Message;
            }
            return td;
        }
    }
}
