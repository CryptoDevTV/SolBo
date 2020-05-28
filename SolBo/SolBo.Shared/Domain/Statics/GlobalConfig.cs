using System;
using System.IO;

namespace SolBo.Shared.Domain.Statics
{
    public static class GlobalConfig
    {
        public static string PriceFile(string symbol)
            => Path.Combine(Directory.GetCurrentDirectory(), $"{symbol}.txt");

        public static string AppFile(string symbol)
            => Path.Combine(Directory.GetCurrentDirectory(), $"{symbol}.json");

        public static string BackupPriceFile(string symbol)
        {
            var backupFileName = $"{symbol}_backup_{string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)}";
            return Path.Combine(Directory.GetCurrentDirectory(), $"{backupFileName}.txt");
        }
    }
}