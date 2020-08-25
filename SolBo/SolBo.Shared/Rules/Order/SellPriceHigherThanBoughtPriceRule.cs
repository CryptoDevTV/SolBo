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
                    ? $"CURRENT PRICE => ({solbot.Communication.Price.Current}) > ({solbot.Actions.BoughtPrice})"
                    : $"CURRENT PRICE => ({solbot.Communication.Price.Current}) < ({solbot.Actions.BoughtPrice})"
            };
        }
    }
}