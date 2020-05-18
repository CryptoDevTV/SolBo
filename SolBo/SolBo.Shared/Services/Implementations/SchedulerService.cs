using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services.Responses;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Shared.Services.Implementations
{
    public class SchedulerService : ISchedulerService
    {
        private readonly JsonSerializerOptions _options;
        public SchedulerService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }
        public async Task<SchedulerResponse> GetConfigAsync(string fileName)
        {
            var result = new SchedulerResponse();

            using (FileStream fs = File.OpenRead(SetPath(fileName)))
            {
                result.SolBotConfig = await JsonSerializer.DeserializeAsync<Solbot>(fs, _options);
            }

            return result;
        }
        public async Task<SchedulerResponse> SetConfigAsync(string fileName, Solbot solbot)
        {
            var result = new SchedulerResponse();

            using (FileStream fs = File.Create(SetPath(fileName)))
            {
                await JsonSerializer.SerializeAsync(fs, solbot, options: _options);
            }

            result.WriteSuccess = true;

            return result;
        }
        private string SetPath(string fileName)
            => Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.json");
    }
}