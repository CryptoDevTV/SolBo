using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Strategies;
using SolBo.Shared.Strategies.Predefined.Exchanges;

namespace Solbo.Strategy.Beta.Models
{
    public class StrategyRootExchange : IStrategyExchange
    {
        public ExchangeType ActiveExchangeType { get; set; }
        public StrategyExchangeKucoin Kucoin { get; set; }
    }
}