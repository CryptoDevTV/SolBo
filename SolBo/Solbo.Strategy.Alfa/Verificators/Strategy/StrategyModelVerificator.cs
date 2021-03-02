using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Validators.Strategy;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Verificators.Strategy
{
    internal class StrategyModelVerificator : IAlfaRule
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