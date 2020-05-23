using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services.Responses;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Shared.Services.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly JsonSerializerOptions _options;
        public ConfigurationService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }
        public async Task<ConfigurationResponse> GetConfigAsync(string fileName)
        {
            var result = new ConfigurationResponse();

            using (FileStream fs = File.OpenRead(GlobalConfig.AppFile(fileName)))
            {
                result.SolBotConfig = await JsonSerializer.DeserializeAsync<Solbot>(fs, _options);
            }

            return result;
        }
        public async Task<ConfigurationResponse> SetConfigAsync(string fileName, Solbot solbot)
        {
            var result = new ConfigurationResponse();

            using (FileStream fs = File.Create(GlobalConfig.AppFile(fileName)))
            {
                await JsonSerializer.SerializeAsync(fs, solbot, options: _options);
            }

            result.WriteSuccess = true;

            return result;
        }
    }
}