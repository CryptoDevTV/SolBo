using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Strategies.Predefined.Exchanges
{
    public class StrategyExchangeBinance
    {
        public ExchangeType ExchangeType => ExchangeType.Binance;
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public override string ToString()
            => $"ApiKey: {ApiKey} | ApiSecret: {ApiSecret}";
    }
}