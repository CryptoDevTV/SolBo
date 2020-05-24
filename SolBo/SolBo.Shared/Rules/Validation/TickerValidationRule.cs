using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Validation
{
    public class TickerValidationRule : IValidatedRule
    {
        public string RuleAttribute => "Ticker";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                solbot.Strategy.AvailableStrategy.TickerType.GetDescription());
        public bool RulePassed(Solbot solbot)
            => Enum.IsDefined(typeof(TickerType), solbot.Strategy.AvailableStrategy.TickerType);
    }
}