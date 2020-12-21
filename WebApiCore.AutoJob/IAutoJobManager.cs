using System.Threading.Tasks;

namespace WebApiCore.AutoJob
{
    public interface IAutoJobManager
    {
        Task<string> Start();
        Task<string> Stop();
    }
}
