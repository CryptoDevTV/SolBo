using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class BuyExecuteTestRule : IRule
    {
        public string RuleName => "BUY";

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
                    ? $"{RuleName} executed"
                    : $"{RuleName} not executed"
            };
        }

        public bool RulePassed(Solbot solbot)
            => solbot.Communication.Buy.PriceReached
            && solbot.Actions.Bought == 0;
    }
}