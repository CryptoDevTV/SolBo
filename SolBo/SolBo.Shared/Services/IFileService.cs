using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Shared.Services
{
    public interface IFileService
    {
        Task SerializeAsync<T>(string fileName, T objectToSerialize);
        Task<T> DeserializeAsync<T>(string fileName);
        bool Exist(string path);
        void CreateBackup(string path, string backupPath);
        void ClearFile(string path);
        void SaveValue(string filePath, decimal val);
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
        public async Task<T> DeserializeAsync<T>(string fileName)
        {
            using FileStream fs = File.OpenRead(fileName);
            return await JsonSerializer.DeserializeAsync<T>(fs, _options);
        }
        public async Task SerializeAsync<T>(string fileName, T objectToSerialize)
        {
            using FileStream fs = File.Create(fileName);
            await JsonSerializer.SerializeAsync(fs, objectToSerialize, options: _options);
        }
        public bool Exist(string path)
            => File.Exists(path);
        public void CreateBackup(string path, string backupPath)
        {
            File.Copy(path, backupPath, true);
        }
        public void ClearFile(string path)
        {
            FileStream fileStream = File.Open(path, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close(); // This flushes the content, too.
        }
        public void SaveValue(string filePath, decimal val)
        {
            using StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(val);
        }
    }
}