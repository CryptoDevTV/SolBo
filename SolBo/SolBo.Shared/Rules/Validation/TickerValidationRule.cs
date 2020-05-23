using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class TickerValidationRule : IValidatedRule
    {
        public string RuleAttribute => "Ticker";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.Ticker}");
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.Ticker == 0
            || solbot.Strategy.AvailableStrategy.Ticker == 1;
    }
}