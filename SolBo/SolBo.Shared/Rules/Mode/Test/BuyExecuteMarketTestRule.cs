using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class BuyExecuteMarketTestRule : IMarketRule
    {
        public string OrderName => "BUY";
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
                    ? $"{OrderName} => Price reached ({solbot.Communication.Buy.PriceReached}), bought before ({solbot.Actions.Bought}), buying ({solbot.Strategy.AvailableStrategy.Symbol}), using ({solbot.Strategy.AvailableStrategy.FundPercentage}%)"
                    : $"{OrderName} => Price reached ({solbot.Communication.Buy.PriceReached}), bought before ({solbot.Actions.Bought})"
            };
        }
    }
}