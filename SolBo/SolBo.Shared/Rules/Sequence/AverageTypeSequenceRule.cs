using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class AverageTypeSequenceRule : ISequencedRule
    {
        public string SequenceName => "AverageType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => new SequencedRuleResult
            {
                Success = Enum.IsDefined(typeof(AverageType), solbot.Strategy.AvailableStrategy.AverageType),
                Message = solbot.Strategy.AvailableStrategy.AverageType == AverageType.WITH_CURRENT
                    ? LogGenerator.AverageTypeSuccess(SequenceName, solbot.Strategy.AvailableStrategy.AverageType.GetDescription())
                    : LogGenerator.AverageTypeError(SequenceName, solbot.Strategy.AvailableStrategy.AverageType.GetDescription())
            };
    }
}