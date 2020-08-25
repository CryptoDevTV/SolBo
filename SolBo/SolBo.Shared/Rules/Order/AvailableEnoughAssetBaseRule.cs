using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableEnoughAssetBaseRule : IOrderRule
    {
        public string OrderStep => "AvailableEnoughAssetBase";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var boughtBefore = solbot.Actions.BoughtPrice > 0;

            var response = boughtBefore 
                ? solbot.Communication.AvailableAsset.Base > solbot.Communication.Symbol.MinNotional
                : false;

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"BASE => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.AvailableAsset.Base})" +
                    $" => ENOUGH"
                    : boughtBefore
                    ?
                    $"BASE => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.AvailableAsset.Base})" +
                    $" => NOT ENOUGH => MIN NEEDED => ({solbot.Communication.Symbol.BaseAsset}:{solbot.Communication.Symbol.MinQuantity})"
                    : $"BASE => NOT BOUGHT"
            };
        }
    }
}