using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Validators.Strategy;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Verificators.Strategy
{
    internal class StrategyModelVerificator : IAlfaRule
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