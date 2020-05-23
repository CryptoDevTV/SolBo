using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossStepValidationRule : IValidatedRule
    {
        public string RuleAttribute => "StopLossPercentageDown";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.StopLossPercentageDown}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.StopLossPercentageDown >= 0;
    }
}