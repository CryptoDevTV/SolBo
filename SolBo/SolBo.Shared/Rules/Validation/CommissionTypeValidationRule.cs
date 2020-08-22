using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Validation
{
    public class CommissionTypeValidationRule : IValidatedRule
    {
        public string RuleAttribute => "CommissionType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                solbot.Strategy.AvailableStrategy.CommissionType.GetDescription());
        public bool RulePassed(Solbot solbot)
            => Enum.IsDefined(typeof(CommissionType), solbot.Strategy.AvailableStrategy.CommissionType);
    }
}