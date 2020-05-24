using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Validation
{
    public class ModeTypeValidationRule : IValidatedRule
    {
        public string RuleAttribute => "ModeType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                solbot.Strategy.ModeType.GetDescription());
        public bool RulePassed(Solbot solbot)
            => Enum.IsDefined(typeof(ModeType), solbot.Strategy.ModeType);
    }
}