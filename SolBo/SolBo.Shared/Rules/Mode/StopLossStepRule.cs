using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode
{
    public class StopLossStepRule : IRule
    {
        private readonly IMarketService _marketService;
        public StopLossStepRule(IMarketService marketService)
        {
            _marketService = marketService;
        }

        public string RuleName => "STOP LOSS STEP";

        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} reached"
                    : $"{RuleName} not reached"
            };
        }

        public bool RulePassed(Solbot solbot)
        {
            var result = _marketService.IsStopLossReached(
                solbot.Strategy.AvailableStrategy.StopLossPercentageDown,
                solbot.Communication.Average.Current,
                solbot.Communication.Price.Current);

            solbot.Communication.StopLoss = new PercentageMessage
            {
                Change = result.PercentChanged,
                PriceReached = result.IsReadyForMarket
            };

            return result.IsReadyForMarket;
        }
    }
}