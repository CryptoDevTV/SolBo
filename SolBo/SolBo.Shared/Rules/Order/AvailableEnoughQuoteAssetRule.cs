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
                    ? $"Available quote assets ({solbot.Communication.Buy.AvailableFund}) are enough to proceed"
                    : $"Available quote assets ({solbot.Communication.Buy.AvailableFund}) are NOT enough to proceed" +
                    $", you need at least ({solbot.Communication.Symbol.MinNotional})"
            };
        }
    }
}