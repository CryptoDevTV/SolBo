using SolBo.Shared.Domain.Statics;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolBo.Shared.Services.Implementations
{
    public class FileStorageService : IStorageService
    {
        private string _filePath;

        public void ClearFile(string symbol)
        {
            FileStream fileStream = File.Open(_filePath, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close(); // This flushes the content, too.
        }

        public string CreateBackup(string symbol)
        {
            var backupPath = GlobalConfig.PriceFileBackup("",symbol);
            File.Copy(GlobalConfig.PriceFile("",symbol), backupPath, true);
            return backupPath;
        }

        public bool Exist(string symbol)
            => File.Exists(GlobalConfig.PriceFile("",symbol));

        public ICollection<decimal> GetValues()
        {
            var list = new List<decimal>();
            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(Convert.ToDecimal(line));
                }
            }

            return list;
        }
        public void SaveValue(decimal val)
        {
            using StreamWriter writer = new StreamWriter(_filePath, true);
            writer.WriteLine(val);
        }
        public void SetPath(string path)
        {
            _filePath = path;
        }
    }
}