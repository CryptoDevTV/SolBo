using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Mode
{
    public class SellStepRule : IRule
    {
        private readonly IMarketService _marketService;
        public SellStepRule(IMarketService marketService)
        {
            _marketService = marketService;
        }
        public string RuleName => "SELL STEP";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} REACHED => {Message}"
                    : $"{RuleName} not reached. {Message}"
            };
        }
        public bool RulePassed(Solbot solbot)
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

            Message = result.PercentChanged > 0
                ? $"Price ({solbot.Communication.Price.Current}) increased from the average ({solbot.Communication.Average.Current}) by {Math.Abs(solbot.Communication.Sell.Change)}%"
                : $"Price ({solbot.Communication.Price.Current}) has fallen from the average ({solbot.Communication.Average.Current}) by {Math.Abs(solbot.Communication.Sell.Change)}%";

            return result.IsReadyForMarket;
        }
    }
}