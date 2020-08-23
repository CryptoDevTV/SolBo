using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossStepValidationRule : IValidatedRule
    {
        public string RuleAttribute => "StopLossDown";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.StopLossDown}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.StopLossDown >= 0;
    }
}