using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Validators.Strategy;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Verificators.Strategy
{
    internal class StrategyModelVerificator : IBetaRules
    {
        public IRuleResult Result(StrategyRootModel strategyRootModel)
        {
            var validator = new StrategyModelValidator();
            var errors = string.Empty;
            foreach (var item in strategyRootModel.Pairs)
            {
                var result = validator.Validate(item);
                if (!result.IsValid)
                {
                    errors += $"{item.Symbol} - {result}";
                }
            }
            return new RuleResult(errors);
        }
    }
}