using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableQuoteAssetRule : IOrderRule
    {
        public string OrderStep => "AvailableQuoteAsset";
        public IRuleResult RuleExecuted(Solbot solbot)
            => new OrderRuleResult
            {
                Success = solbot.Communication.Buy.AvailableFund > 0.0m,
                Message = $"QUOTE => ({solbot.Communication.Buy.AvailableFund})"
            };
    }
}