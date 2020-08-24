using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class AvailableAssetBaseRule : IOrderRule
    {
        public string OrderStep => "AvailableAssetBase";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.AvailableAsset.Base > 0.0m;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Base asset ({solbot.Communication.AvailableAsset.Base})"
                    : $"Base asset ({solbot.Communication.AvailableAsset.Base})"
            };
        }
    }
}