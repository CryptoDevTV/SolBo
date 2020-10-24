using System.Collections.Generic;

namespace Solbo.Strategy.Beta.Models
{
    internal class StrategyRootModel
    {
        public IEnumerable<StrategyModel> Pairs { get; set; }
        public StrategyRootExchange Exchange { get; set; }
    }
}