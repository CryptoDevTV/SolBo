using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class SellPriceReachedRule : IOrderRule
    {
        public string OrderStep => "SellPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Sell.PriceReached;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Sell price reached {solbot.Communication.Sell.PriceReached}"
                    : $"Sell price not reached {solbot.Communication.Sell.PriceReached}"
            };
        }
    }
}