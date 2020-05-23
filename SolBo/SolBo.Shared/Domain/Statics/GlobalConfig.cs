using System.IO;

namespace SolBo.Shared.Domain.Statics
{
    public static class GlobalConfig
    {
        public static string PriceFile(string symbol)
            => Path.Combine(Directory.GetCurrentDirectory(), $"{symbol}.txt");

        public static string AppFile(string symbol)
            => Path.Combine(Directory.GetCurrentDirectory(), $"{symbol}.json");
    }
}