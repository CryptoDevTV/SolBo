using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class StopLossExecuteTestRule : IRule
    {
        public string RuleName => "STOP LOSS";
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
            var result = solbot.Communication.StopLoss.PriceReached && solbot.Actions.Bought == 1;

            Message = result 
                ? $"Price reached ({solbot.Communication.StopLoss.PriceReached}), bought before ({solbot.Actions.Bought}), stop loss type ({solbot.Strategy.AvailableStrategy.StopLossType})"
                : $"Price reached ({ solbot.Communication.StopLoss.PriceReached}), bought before ({ solbot.Actions.Bought})";

            return result;
        }
    }
}