using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Validators.Exchange;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Verificators.Exchange
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