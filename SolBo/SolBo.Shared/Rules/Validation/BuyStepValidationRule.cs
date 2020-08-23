using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class BuyStepValidationRule : IValidatedRule
    {
        public string RuleAttribute => "BuyDown";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.BuyDown}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.BuyDown > 0;
    }
}