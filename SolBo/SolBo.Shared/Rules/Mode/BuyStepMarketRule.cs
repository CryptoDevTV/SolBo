using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Mode
{
    public class BuyStepMarketRule : IMarketRule
    {
        private readonly IMarketService _marketService;
        public BuyStepMarketRule(IMarketService marketService)
        {
            _marketService = marketService;
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

            return new MarketRuleResult()
            {
                Success = result.IsReadyForMarket,
                Message = result.PercentChanged < 0
                    ? $"Price ({solbot.Communication.Price.Current}) increased from the average ({solbot.Communication.Average.Current}) by {Math.Abs(solbot.Communication.Buy.Change)}%"
                    : $"Price ({solbot.Communication.Price.Current}) has fallen from the average ({solbot.Communication.Average.Current}) by {Math.Abs(solbot.Communication.Buy.Change)}%"
            };
        }
    }
}