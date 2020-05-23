using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class BuyExecuteMarketTestRule : IMarketRule
    {
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
                    ? $"Price reached ({solbot.Communication.Buy.PriceReached}), bought before ({solbot.Actions.Bought}), buying ({solbot.Strategy.AvailableStrategy.Symbol}), using ({solbot.Strategy.AvailableStrategy.FundPercentage}%)"
                    : $"Price reached ({solbot.Communication.Buy.PriceReached}), bought before ({solbot.Actions.Bought})"
            };
        }
    }
}