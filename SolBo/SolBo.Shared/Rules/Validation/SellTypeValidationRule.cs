using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Validation
{
    public class SellTypeValidationRule : IValidatedRule
    {
        public string RuleAttribute => "SellType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                solbot.Strategy.AvailableStrategy.SellType.GetDescription());
        public bool RulePassed(Solbot solbot)
            => Enum.IsDefined(typeof(SellType), solbot.Strategy.AvailableStrategy.SellType);
    }
}