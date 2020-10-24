using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Validators;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Verificators
{
    internal class StrategyRootExchangeVerificator : IBetaRules
    {
        public IRuleResult Result(StrategyRootModel strategyRootModel)
        {
            var validator = new StrategyRootExchangeValidator();
            return new RuleResult(validator.Validate(strategyRootModel.Exchange));
        }
    }
}