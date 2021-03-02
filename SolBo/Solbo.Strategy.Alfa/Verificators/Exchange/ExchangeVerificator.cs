using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Validators.Exchange;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Verificators.Exchange
{
    internal class ExchangeVerificator : IExchangeRule
    {
        public IRuleResult Result(StrategyRootExchange strategyRootExchange)
        {
            var validator = new ExchangeValidator();
            var result = validator.Validate(strategyRootExchange);

            return new RuleResult(result.ToString());
        }
    }
}