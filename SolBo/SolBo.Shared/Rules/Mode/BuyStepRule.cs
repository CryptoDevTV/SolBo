using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Mode
{
    public class BuyStepRule : IRule
    {
        private readonly IMarketService _marketService;
        public BuyStepRule(IMarketService marketService)
        {
            _marketService = marketService;
        }
        public string RuleName => "BUY STEP";
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
            var result = _marketService.IsGoodToBuy(
                solbot.Strategy.AvailableStrategy.BuyPercentageDown,
                solbot.Communication.Average.Current,
                solbot.Communication.Price.Current);

            solbot.Communication.Buy = new PercentageMessage
            {
                Change = result.PercentChanged,
                PriceReached = result.IsReadyForMarket
            };

            Message = result.PercentChanged < 0
                ? $"Price ({solbot.Communication.Price.Current}) increased from the average ({solbot.Communication.Average.Current}) by {Math.Abs(solbot.Communication.Buy.Change)}%"
                : $"Price ({solbot.Communication.Price.Current}) has fallen from the average ({solbot.Communication.Average.Current}) by {Math.Abs(solbot.Communication.Buy.Change)}%";

            return result.IsReadyForMarket;
        }
    }
}