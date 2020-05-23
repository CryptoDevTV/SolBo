using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class SellExecuteMarketTestRule : IMarketRule
    {
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
                    ? $"Price reached ({solbot.Communication.Sell.PriceReached}), bought before ({solbot.Actions.Bought}), selling ({solbot.Strategy.AvailableStrategy.Symbol})"
                    : $"Price reached ({solbot.Communication.Sell.PriceReached}), bought before ({solbot.Actions.Bought})"
            };
        }
    }
}