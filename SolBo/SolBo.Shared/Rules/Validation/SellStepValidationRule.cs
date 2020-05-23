using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class SellStepValidationRule : IValidatedRule
    {
        public string RuleAttribute => "SellPercentageUp";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.SellPercentageUp}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.SellPercentageUp > 0;
    }
}