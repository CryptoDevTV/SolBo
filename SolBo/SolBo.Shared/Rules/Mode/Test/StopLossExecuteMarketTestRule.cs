using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class StopLossExecuteMarketTestRule : IMarketRule
    {
        public string OrderName => "STOPLOSS";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.StopLoss.PriceReached && solbot.Actions.Bought == 1;

            if (result)
            {
                solbot.Actions.Bought = 0;
                result = true;
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? $"{OrderName} => Price reached ({solbot.Communication.StopLoss.PriceReached}), bought before ({solbot.Actions.Bought}), stop loss type ({solbot.Strategy.AvailableStrategy.StopLossType})"
                    : $"{OrderName} => Price reached ({ solbot.Communication.StopLoss.PriceReached}), bought before ({ solbot.Actions.Bought})"
            };
        }
    }
}