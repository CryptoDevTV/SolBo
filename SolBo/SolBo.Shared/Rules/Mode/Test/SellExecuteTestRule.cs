using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class SellExecuteTestRule : IRule
    {
        public string RuleName => "SELL";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = false;

            if (RulePassed(solbot))
            {
                solbot.Actions.Bought = 0;
                result = true;
            }

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} EXECUTED => {Message}"
                    : $"{RuleName} not executed. {Message}"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            var result = solbot.Communication.Sell.PriceReached && solbot.Actions.Bought == 1;

            Message = result
                ? $"Price reached ({solbot.Communication.Sell.PriceReached}), bought before ({solbot.Actions.Bought}), selling ({solbot.Strategy.AvailableStrategy.Symbol})"
                : $"Price reached ({solbot.Communication.Sell.PriceReached}), bought before ({solbot.Actions.Bought})";

            return result;
        }
    }
}