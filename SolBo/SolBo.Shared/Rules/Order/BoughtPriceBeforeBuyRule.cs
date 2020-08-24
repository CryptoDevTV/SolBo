using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class BoughtPriceBeforeBuyRule : IOrderRule
    {
        public string OrderStep => "BoughtPriceBeforeBuy";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Actions.BoughtPrice == 0;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"WAITING FOR SALE => NO => LAST BUY => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Actions.BoughtPrice})"
                    : $"WAITING FOR SALE => YES => LAST BUY => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Actions.BoughtPrice})"
            };
        }
    }
}