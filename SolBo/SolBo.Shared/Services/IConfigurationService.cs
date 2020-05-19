using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;
using System.Threading.Tasks;

namespace SolBo.Shared.Services
{
    public interface IConfigurationService
    {
        Task<ConfigurationResponse> GetConfigAsync(string fileName);
        Task<ConfigurationResponse> SetConfigAsync(string fileName, Solbot solbot);
    }
}