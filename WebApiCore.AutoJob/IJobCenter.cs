using System.Threading.Tasks;

namespace WebApiCore.AutoJob
{
    public interface IJobCenter
    {
        Task<bool> Start();
        Task<bool> StopAll();
        Task<bool> EditJob(string name, string group);
        Task<bool> RemoveJob(string name, string group);
        Task<bool> AddNewJob(string name, string group);
    }
}
