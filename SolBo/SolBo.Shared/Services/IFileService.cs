using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Shared.Services
{
    public interface IFileService
    {
        Task SetAsync<T>(string fileName, T objectToSerialize);
        Task<T> GetAsync<T>(string fileName);
    }

    public class FileService : IFileService
    {
        private readonly JsonSerializerOptions _options;
        public FileService()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task<T> GetAsync<T>(string fileName)
        {
            using FileStream fs = File.OpenRead(fileName);
            return await JsonSerializer.DeserializeAsync<T>(fs, _options);
        }

        public async Task SetAsync<T>(string fileName, T objectToSerialize)
        {
            using FileStream fs = File.Create(fileName);
            await JsonSerializer.SerializeAsync(fs, objectToSerialize, options: _options);
        }
    }
}