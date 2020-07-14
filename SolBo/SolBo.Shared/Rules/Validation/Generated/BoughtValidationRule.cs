using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation.Generated
{
    public class BoughtValidationRule : IValidatedRule
    {
        public string RuleAttribute => "Bought";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Actions.BoughtPrice}");
        public bool RulePassed(Solbot solbot)
            => solbot.Actions.BoughtPrice >= 0;
    }
}