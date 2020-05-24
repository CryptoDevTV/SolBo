using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossTypeValidationRule : IValidatedRule
    {
        public string RuleAttribute => "StopLossType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                solbot.Strategy.AvailableStrategy.StopLossType.GetDescription());
        public bool RulePassed(Solbot solbot)
            => Enum.IsDefined(typeof(StopLossType), solbot.Strategy.AvailableStrategy.StopLossType);
    }
}