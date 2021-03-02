using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Validators.Exchange;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Verificators.Exchange
{
    internal class ExchangeBinanceVerificator : IExchangeRule
    {
        public IRuleResult Result(StrategyRootExchange strategyRootExchange)
        {
            var validator = new ExchangeBinanceValidator();
            var result = validator.Validate(strategyRootExchange.Binance);

            return new RuleResult(result.ToString());
        }
    }
}