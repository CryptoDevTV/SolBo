using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class BuyStepValidationRule : IValidatedRule
    {
        public string RuleAttribute => "BuyPercentageDown";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.BuyPercentageDown}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.BuyPercentageDown > 0;
    }
}