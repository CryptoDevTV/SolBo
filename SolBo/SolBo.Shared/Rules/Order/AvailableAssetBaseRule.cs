using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableAssetBaseRule : IOrderRule
    {
        public string OrderStep => "AvailableAssetBase";
        public IRuleResult RuleExecuted(Solbot solbot)
            => new OrderRuleResult
            {
                Success = solbot.Communication.AvailableAsset.Base > 0.0m,
                Message = $"BASE => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.AvailableAsset.Base})"
            };
    }
}