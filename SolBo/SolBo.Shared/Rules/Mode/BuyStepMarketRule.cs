using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode
{
    public class BuyStepMarketRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly IMarketService _marketService;
        private readonly bool _isInProductionMode;
        public BuyStepMarketRule(IMarketService marketService, bool isInProductionMode)
        {
            _marketService = marketService;
            _isInProductionMode = isInProductionMode;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = _marketService.IsGoodToBuy(
                solbot.Strategy.AvailableStrategy.BuyPercentageDown,
                solbot.Communication.Average.Current,
                solbot.Communication.Price.Current);

            solbot.Communication.Buy = new PercentageMessage
            {
                Change = result.PercentChanged,
                PriceReached = result.IsReadyForMarket
            };

            if (_isInProductionMode)
            {
                var fundResponse = _marketService.AvailableQuote(solbot.Strategy.AvailableStrategy.FundPercentage, solbot.Communication.AvailableAsset.Quote, solbot.Communication.Symbol.QuoteAssetPrecision);

                solbot.Communication.Buy.AvailableFund = fundResponse.QuoteAssetToTrade;
            }

            return new MarketRuleResult()
            {
                Success = result.IsReadyForMarket,
                Message = result.PercentChanged < 0
                    ? LogGenerator.StepMarketSuccess(MarketOrder, solbot.Communication.Price.Current, solbot.Communication.Average.Current, solbot.Communication.Buy.Change)
                    : LogGenerator.StepMarketError(MarketOrder, solbot.Communication.Price.Current, solbot.Communication.Average.Current, solbot.Communication.Buy.Change)
            };
        }
    }
}