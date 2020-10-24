using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Strategies;
using SolBo.Shared.Strategies.Exchanges;

namespace Solbo.Strategy.Alfa.Models
{
    public class StrategyRootExchange : IStrategyExchange
    {
        public ExchangeType ActiveExchangeType { get; set; }
        public StrategyExchangeBinance Binance { get; set; }
    }
}