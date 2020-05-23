using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StrategyValidationRule : IValidatedRule
    {
        public string RuleAttribute => "ActiveId";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.ActiveId}");
        public bool RulePassed(Solbot solbot)
            => !(solbot.Strategy.AvailableStrategy is null);
    }
}