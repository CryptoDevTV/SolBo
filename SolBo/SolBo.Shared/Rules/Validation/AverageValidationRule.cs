using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class AverageValidationRule : IValidatedRule
    {
        public string RuleAttribute => "Average";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot), 
                RuleAttribute, 
                $"{solbot.Strategy.AvailableStrategy.Average}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.Average > 0;
    }
}