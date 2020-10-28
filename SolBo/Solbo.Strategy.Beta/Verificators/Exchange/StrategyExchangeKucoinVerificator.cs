using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Validators.Exchange;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Verificators.Exchange
{
    internal class StrategyExchangeKucoinVerificator : IBetaRules
    {
        public IRuleResult Result(StrategyRootModel strategyRootModel)
        {
            var validator = new StrategyExchangeKucoinValidator();
            var result = validator.Validate(strategyRootModel.Exchange.Kucoin);

            return new RuleResult(result.ToString());
        }
    }
}