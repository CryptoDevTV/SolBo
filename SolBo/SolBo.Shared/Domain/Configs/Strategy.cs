using System.Collections.Generic;

namespace SolBo.Shared.Domain.Configs
{
    public class Strategy
    {
        public string Name { get; set; }
        public IEnumerable<Pair> Pairs { get; set; }
    }
}