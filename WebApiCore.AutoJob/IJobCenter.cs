using System.Threading.Tasks;

namespace WebApiCore.AutoJob
{
    public interface IJobCenter
    {
        Task<string> Start();
        Task<string> StopAll();
        Task<string> Edit(string name, string group);
        Task<string> Remove(string name, string group);
        Task<string> Add(string name, string group);
    }
}
