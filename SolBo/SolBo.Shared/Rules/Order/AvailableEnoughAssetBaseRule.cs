using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableEnoughAssetBaseRule : IOrderRule
    {
        public string OrderStep => "AvailableEnoughAssetBase";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.AvailableAsset.Base > solbot.Communication.Symbol.MinNotional;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Available base asset ({solbot.Communication.AvailableAsset.Base}) are enough to proceed"
                    : $"Available base asset ({solbot.Communication.AvailableAsset.Base}) are NOT enough to proceed" +
                    $", you need at least ({solbot.Communication.Symbol.MinNotional})"
            };
        }
    }
}