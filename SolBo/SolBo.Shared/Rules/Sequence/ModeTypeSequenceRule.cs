using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;

namespace SolBo.Shared.Rules.Sequence
{
    public class ModeTypeSequenceRule : ISequencedRule
    {
        public string SequenceName => "ModeType";
        public IRuleResult RuleExecuted(Solbot solbot)
            => new SequencedRuleResult
            {
                Success = solbot.Strategy.ModeType == ModeType.WORKING,
                Message = solbot.Strategy.ModeType == ModeType.WORKING
                    ? LogGenerator.ModeTypeSuccess(SequenceName, solbot.Strategy.ModeType.GetDescription())
                    : LogGenerator.ModeTypeSuccess(SequenceName, solbot.Strategy.ModeType.GetDescription())
            };
    }
}