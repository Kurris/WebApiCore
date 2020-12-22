using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Interface.SystemManage
{
    public interface IAutoJobService
    {
        Task<IEnumerable<AutoJob>> GetJobList();
        Task<IEnumerable<AutoJob>> GetActiveJobList();
        Task<AutoJob> GetAutoJob(string name, string group);
        Task<TData<string>> AddJob(AutoJob autoJob);
        Task<TData<string>> EditJob(AutoJob autoJob);
        Task<TData<string>> RemoveJob(int id);
        Task ExecProc(string procedure, string[] args);
    }
}
