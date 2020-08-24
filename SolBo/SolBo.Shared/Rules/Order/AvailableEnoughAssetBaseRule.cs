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
                    ? $"BASE => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.AvailableAsset.Base})" +
                    $" => ENOUGH"
                    : $"BASE => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.AvailableAsset.Base})" +
                    $" => NOT ENOUGH => MIN NEEDED => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.Symbol.MinQuantity})"
            };
        }
    }
}