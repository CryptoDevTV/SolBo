using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Validators.Strategy;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Verificators.Strategy
{
    internal class StrategyModelVerificator : IBetaRule
    {
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var validator = new StrategyModelValidator();
            var errors = string.Empty;
            var result = validator.Validate(strategyModel);
            if (!result.IsValid)
            {
                errors = $"{result}";
            }
            return new RuleResult(errors);
        }
    }
}