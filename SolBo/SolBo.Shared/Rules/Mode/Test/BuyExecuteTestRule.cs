using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class BuyExecuteTestRule : IRule
    {
        public string RuleName => "BUY";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = false;

            if(RulePassed(solbot))
            {
                solbot.Actions.Bought = 1;
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
            var result = solbot.Communication.Buy.PriceReached && solbot.Actions.Bought == 0;

            Message = result
                ? $"Price reached ({solbot.Communication.Buy.PriceReached}), bought before ({solbot.Actions.Bought}), buying ({solbot.Strategy.AvailableStrategy.Symbol}), using ({solbot.Strategy.AvailableStrategy.FundPercentage}%)"
                : $"Price reached ({solbot.Communication.Buy.PriceReached}), bought before ({solbot.Actions.Bought})";

            return result;
        }
    }
}