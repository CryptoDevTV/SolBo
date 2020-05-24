﻿using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode
{
    public class SellStepMarketRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;
        private readonly IMarketService _marketService;
        public SellStepMarketRule(IMarketService marketService)
        {
            _marketService = marketService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = _marketService.IsGoodToSell(
                solbot.Strategy.AvailableStrategy.SellPercentageUp,
                solbot.Communication.Average.Current,
                solbot.Communication.Price.Current);

            solbot.Communication.Sell = new PercentageMessage
            {
                Change = result.PercentChanged,
                PriceReached = result.IsReadyForMarket
            };

            return new MarketRuleResult()
            {
                Success = result.IsReadyForMarket,
                Message = result.PercentChanged > 0
                    ? LogGenerator.StepMarketSuccess(MarketOrder, solbot.Communication.Price.Current, solbot.Communication.Average.Current, solbot.Communication.Sell.Change)
                    : LogGenerator.StepMarketError(MarketOrder, solbot.Communication.Price.Current, solbot.Communication.Average.Current, solbot.Communication.Sell.Change)
            };
        }
    }
}