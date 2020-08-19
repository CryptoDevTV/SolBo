using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableQuoteAssetRule : IOrderRule
    {
        public string OrderStep => "AvailableQuoteAsset";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Buy.AvailableFund > 0.0m;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Quote asset ({solbot.Communication.Buy.AvailableFund}) - OK"
                    : $"Quote asset ({solbot.Communication.Buy.AvailableFund}) - ERROR"
            };
        }
    }
}