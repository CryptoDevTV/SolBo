using SolBo.Shared.Strategies;
using System.Collections.Generic;

namespace Solbo.Strategy.Alfa.Models
{
    internal class StrategyRootModel : IStrategyRootModel
    {
        public IEnumerable<StrategyModel> Pairs { get; set; }
        public StrategyRootExchange Exchange { get; set; }
    }
}