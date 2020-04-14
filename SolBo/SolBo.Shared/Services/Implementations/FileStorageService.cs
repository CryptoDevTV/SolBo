using System;
using System.Collections.Generic;
using System.IO;

namespace SolBo.Shared.Services.Implementations
{
    public class FileStorageService : IStorageService
    {
        private readonly string _filePath;

        public FileStorageService(string filePath)
        {
            _filePath = filePath;
        }

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
    }
}