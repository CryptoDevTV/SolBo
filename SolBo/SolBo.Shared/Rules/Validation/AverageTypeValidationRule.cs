using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Validation
{
    public class AverageTypeValidationRule : IValidatedRule
    {
        public string RuleAttribute => "AverageType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                solbot.Strategy.AvailableStrategy.AverageType.GetDescription());
        public bool RulePassed(Solbot solbot)
            => Enum.IsDefined(typeof(AverageType), solbot.Strategy.AvailableStrategy.AverageType);
    }
}