using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class BuyExecuteMarketTestRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.Buy.PriceReached && solbot.Actions.Bought == 0;

            if (result)
            {
                solbot.Actions.Bought = 1;
                result = true;
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.ExecuteMarketSuccess(MarketOrder, solbot.Communication.Buy.PriceReached, solbot.Actions.Bought)
                    : LogGenerator.ExecuteMarketError(MarketOrder, solbot.Communication.Buy.PriceReached, solbot.Actions.Bought)
            };
        }
    }
}