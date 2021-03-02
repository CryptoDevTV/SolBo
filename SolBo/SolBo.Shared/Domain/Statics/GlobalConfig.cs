using System;
using System.IO;

namespace SolBo.Shared.Domain.Statics
{
    public static class GlobalConfig
    {
        public static string PriceFile(string strategy, string symbol)
            => Path.Combine(Directory.GetCurrentDirectory(), "strategies", strategy, $"{symbol}.txt");
        public static string PriceFileBackup(string strategy, string symbol)
        {
            var backupFileName = $"{symbol}_backup_{string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)}";
            return Path.Combine(Directory.GetCurrentDirectory(), "strategies", strategy, $"{backupFileName}.txt");
        }
        public static string AppFile(string symbol)
            => Path.Combine(Directory.GetCurrentDirectory(), $"{symbol}.json");

        public static int RoundValue => 8;
    }
}