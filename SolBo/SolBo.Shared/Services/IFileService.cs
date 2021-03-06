using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Shared.Services
{
    public interface IFileService
    {
        Task SerializeAsync<T>(string pathToFile, T objectToSerialize);
        Task<T> DeserializeAsync<T>(string pathToFile);
        bool Exist(string path);
        void CreateBackup(string path, string backupPath);
        void ClearFile(string path);
        void SaveValue(string filePath, decimal val);
        IEnumerable<decimal> GetValues(string filePath, int numbersToTake = 0);
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
        public async Task<T> DeserializeAsync<T>(string pathToFile)
        {
            using FileStream fs = File.OpenRead(pathToFile);
            return await JsonSerializer.DeserializeAsync<T>(fs, _options);
        }
        public async Task SerializeAsync<T>(string pathToFile, T objectToSerialize)
        {
            using FileStream fs = File.Create(pathToFile);
            await JsonSerializer.SerializeAsync(fs, objectToSerialize, options: _options);
        }
        public bool Exist(string pathToFile)
            => File.Exists(pathToFile);
        public void CreateBackup(string pathToFile, string backupPath)
        {
            File.Copy(pathToFile, backupPath, true);
        }
        public void ClearFile(string pathToFile)
        {
            FileStream fileStream = File.Open(pathToFile, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close(); // This flushes the content, too.
        }
        public void SaveValue(string filePath, decimal val)
        {
            using StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(val);
        }
        public IEnumerable<decimal> GetValues(string pathToFile, int numbersToTake = 0)
        {
            var queue = new Queue<decimal>(numbersToTake);

            using FileStream fs = File.Open(pathToFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            using BufferedStream bs = new BufferedStream(fs);
            using StreamReader sr = new StreamReader(bs);
            while (!sr.EndOfStream)
            {
                if (queue.Count == numbersToTake)
                {
                    queue.Dequeue();
                }

                queue.Enqueue(Convert.ToDecimal(sr.ReadLine()));
            }
            return queue.ToList();
        }
    }
}