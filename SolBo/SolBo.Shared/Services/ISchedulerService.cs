using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;
using System.Threading.Tasks;

namespace SolBo.Shared.Services
{
    public interface ISchedulerService
    {
        Task<SchedulerResponse> GetConfigAsync(string fileName);
        Task<SchedulerResponse> SetConfigAsync(string fileName, Solbot solbot);
    }
}