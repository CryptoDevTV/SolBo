using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class BoughtPriceBeforeSellAndStopLossRule : IOrderRule
    {
        public string OrderStep => "BoughtPriceBeforeSellAndStopLoss";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Actions.BoughtPrice > 0;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"ABLE TO => LAST BUY => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Actions.BoughtPrice})"
                    : $"NOT ABLE TO => LAST BUY => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Actions.BoughtPrice})"
            };
        }
    }
}