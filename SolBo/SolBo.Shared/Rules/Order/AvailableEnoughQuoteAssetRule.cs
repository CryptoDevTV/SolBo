using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableEnoughQuoteAssetRule : IOrderRule
    {
        public string OrderStep => "AvailableEnoughQuoteAsset";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Buy.AvailableFund > solbot.Communication.Symbol.MinNotional;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"QUOTE => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Communication.Buy.AvailableFund})" +
                    $" => ENOUGH"
                    : $"QUOTE => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Communication.Buy.AvailableFund})" +
                    $" => NOT ENOUGH => MIN NEEDED => ({solbot.Communication.Symbol.QuoteAsset}:{solbot.Communication.Symbol.MinNotional})"
            };
        }
    }
}