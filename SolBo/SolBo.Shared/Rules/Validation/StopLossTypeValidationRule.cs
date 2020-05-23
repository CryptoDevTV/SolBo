using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossTypeValidationRule : IValidatedRule
    {
        public string RuleAttribute => "StopLossType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.StopLossType}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.StopLossType == 0
            || solbot.Strategy.AvailableStrategy.StopLossType == 1;
    }
}