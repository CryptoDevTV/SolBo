using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class FundStepValidationRule : IValidatedRule
    {
        public string RuleAttribute => "FundPercentage";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.FundPercentage}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.FundPercentage > 0
            && solbot.Strategy.AvailableStrategy.FundPercentage < 100;
    }
}