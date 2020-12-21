using System.Threading.Tasks;

namespace WebApiCore.AutoJob
{
    public interface IJobCenter
    {
        Task<string> Start();
        Task<string> Stop();
    }
}
