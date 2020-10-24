using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Strategies.Predefined.Exchanges
{
    public class StrategyExchangeKucoin
    {
        public ExchangeType ExchangeType => ExchangeType.KuCoin;
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string PassPhrase { get; set; }
        public override string ToString()
            => $"ApiKey: {ApiKey} | ApiSecret: {ApiSecret} | PassPhrase: {PassPhrase}";
    }
}