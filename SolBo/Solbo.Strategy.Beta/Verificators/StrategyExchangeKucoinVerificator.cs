using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Validators;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Verificators
{
    internal class StrategyExchangeKucoinVerificator : IBetaRules
    {
        public IRuleResult Result(StrategyRootModel strategyRootModel)
        {
            var validator = new StrategyExchangeKucoinValidator();
            return new RuleResult(validator.Validate(strategyRootModel.Exchange.Kucoin));
        }
    }
}