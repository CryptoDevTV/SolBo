using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class StopLossExecuteMarketTestRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.STOPLOSS;
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.StopLoss.PriceReached && solbot.Actions.Bought == 1;

            if (result)
            {
                solbot.Actions.Bought = 0;
                solbot.Actions.StopLossReached = true;
                result = true;
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.ExecuteMarketSuccess(MarketOrder, solbot.Communication.StopLoss.PriceReached, solbot.Actions.Bought)
                    : LogGenerator.ExecuteMarketError(MarketOrder, solbot.Communication.StopLoss.PriceReached, solbot.Actions.Bought)
            };
        }
    }
}