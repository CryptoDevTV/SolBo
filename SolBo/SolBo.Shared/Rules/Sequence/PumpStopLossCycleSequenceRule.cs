using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;

namespace SolBo.Shared.Rules.Sequence
{
    public class PumpStopLossCycleSequenceRule : ISequencedRule
    {
        public string SequenceName => "PumpStopLossCycle";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            if (solbot.Actions.StopLossReached
                && solbot.Actions.Bought == 1
                && solbot.Actions.StopLossCurrentCycle < solbot.Strategy.AvailableStrategy.StopLossPauseCycles)
            {
                solbot.Actions.StopLossCurrentCycle++;

                result.Success = false;
                result.Message = LogGenerator.SequenceError(SequenceName, $"{solbot.Actions.StopLossCurrentCycle}/{solbot.Strategy.AvailableStrategy.StopLossPauseCycles}");
            }
            else if(!solbot.Actions.StopLossReached
                && solbot.Actions.Bought == 1)
            {
                result.Success = true;
                result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{solbot.Actions.StopLossCurrentCycle}");
            }
            else
            {
                solbot.Actions.StopLossCurrentCycle = 0;
                solbot.Actions.Bought = 0;
                solbot.Actions.StopLossReached = false;

                result.Success = true;
                result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{solbot.Actions.StopLossCurrentCycle}");
            }
            return result;
        }
    }
}