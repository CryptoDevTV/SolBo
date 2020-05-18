using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class StopLossExecuteTestRule : IRule
    {
        public string RuleName => "STOP LOSS";

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
                    ? $"{RuleName} executed"
                    : $"{RuleName} not executed"
            };
        }

        public bool RulePassed(Solbot solbot)
            => solbot.Communication.StopLoss.PriceReached
            && solbot.Actions.Bought == 1;
    }
}