using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class SellExecuteMarketTestRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.Sell.PriceReached && solbot.Actions.Bought == 1;

            if (result)
            {
                solbot.Actions.Bought = 0;
                result = true;
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.ExecuteMarketSuccess(MarketOrder, solbot.Communication.Sell.PriceReached, solbot.Actions.Bought)
                    : LogGenerator.ExecuteMarketError(MarketOrder, solbot.Communication.Sell.PriceReached, solbot.Actions.Bought)
            };
        }
    }
}