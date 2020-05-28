using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation.Generated
{
    public class StopLossCurrentCycleValidationRule : IValidatedRule
    {
        public string RuleAttribute => "StopLossCurrentCycle";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Actions.StopLossCurrentCycle}");
        public bool RulePassed(Solbot solbot)
            => solbot.Actions.StopLossCurrentCycle >= 0
                && solbot.Actions.StopLossCurrentCycle <= solbot.Strategy.AvailableStrategy.StopLossPauseCycles;
    }
}