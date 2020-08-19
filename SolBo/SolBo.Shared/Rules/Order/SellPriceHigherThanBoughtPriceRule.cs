using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class SellPriceHigherThanBoughtPriceRule : IOrderRule
    {
        public string OrderStep => "SellPriceHigherThanBoughtPrice";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Price.Current > solbot.Actions.BoughtPrice;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? "Current price for sell is higher than bought price, ready to sell"
                    : "Current price for sell is lower than bought price, not ready to sell"
            };
        }
    }
}