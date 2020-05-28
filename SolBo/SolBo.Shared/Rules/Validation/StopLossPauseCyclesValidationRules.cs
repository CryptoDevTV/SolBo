using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossPauseCyclesValidationRules : IValidatedRule
    {
        public string RuleAttribute => "StopLossPauseCycles";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.StopLossPauseCycles}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.StopLossPauseCycles >= 0;
    }
}