using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class BuyPriceReachedRule : IOrderRule
    {
        public string OrderStep => "BuyPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Buy.PriceReached;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? "Bought price reached"
                    : "Bought price not reached"
            };
        }
    }
}