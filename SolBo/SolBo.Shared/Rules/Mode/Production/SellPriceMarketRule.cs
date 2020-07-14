using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class SellPriceMarketRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;

        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.AvailableAsset.Base > 0.0m &&
                solbot.Communication.AvailableAsset.Base > solbot.Communication.Symbol.MinNotional &&
                solbot.Communication.Sell.PriceReached && solbot.Actions.BoughtPrice > 0 &&
                solbot.Communication.Price.Current > solbot.Actions.BoughtPrice;

            solbot.Communication.Sell.IsReady = result;

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.PriceMarketSuccess(MarketOrder)
                    : LogGenerator.PriceMarketError(MarketOrder)
            };
        }
    }
}