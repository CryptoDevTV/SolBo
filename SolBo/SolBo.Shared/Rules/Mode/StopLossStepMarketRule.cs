using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode
{
    public class StopLossStepMarketRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.STOPLOSS;
        private readonly IMarketService _marketService;
        public StopLossStepMarketRule(IMarketService marketService)
        {
            _marketService = marketService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            if (solbot.Strategy.AvailableStrategy.StopLossPercentageDown == 0)
            {
                solbot.Communication.StopLoss = new PercentageMessage
                {
                    Change = 0,
                    PriceReached = false
                };

                return new MarketRuleResult()
                {
                    Success = false,
                    Message = LogGenerator.Off(MarketOrder)
                };
            }
            else
            {
                var boughtPrice = solbot.Strategy.AvailableStrategy.SellType == SellType.FROM_AVERAGE_VALUE
                    ? solbot.Communication.Average.Current
                    : solbot.Actions.BoughtPrice;

                var result = _marketService.IsStopLossReached(
                    solbot.Strategy.AvailableStrategy.StopLossPercentageDown,
                    boughtPrice,
                    solbot.Communication.Price.Current);

                solbot.Communication.StopLoss = new PercentageMessage
                {
                    Change = result.PercentChanged,
                    PriceReached = result.IsReadyForMarket
                };

                return new MarketRuleResult()
                {
                    Success = result.IsReadyForMarket,
                    Message = result.PercentChanged < 0
                        ? LogGenerator.StepMarketSuccess(MarketOrder, solbot.Communication.Price.Current, boughtPrice, solbot.Communication.StopLoss.Change)
                        : LogGenerator.StepMarketError(MarketOrder, solbot.Communication.Price.Current, boughtPrice, solbot.Communication.StopLoss.Change)
                };
            }
        }
    }
}