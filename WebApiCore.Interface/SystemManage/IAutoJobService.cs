using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore.Entity.SystemManage;
using WebApiCore.Utils.Model;

namespace WebApiCore.Interface.SystemManage
{
    public interface IAutoJobService
    {
        Task<IEnumerable<AutoJobTask>> GetJobList();
        Task<IEnumerable<AutoJobTask>> GetActiveJobList();
        Task<AutoJobTask> GetAutoJob(string name, string group);
        Task<TData<string>> AddJob(AutoJobTask autoJob);
        Task<TData<string>> EditJob(AutoJobTask autoJob);
        Task<TData<string>> RemoveJob(int id);
        Task ExecProc(string procedure, string[] args);

        Task StopAll();
    }
}
