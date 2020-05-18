using Binance.Net.Interfaces;
using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Messages.Rules;

namespace SolBo.Shared.Rules.Market
{
    public class GetPriceRule : IRule
    {
        private readonly IBinanceClient _binanceClient;
        public GetPriceRule(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public string RuleName => "PRICE GATHERING";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} success"
                    : $"{RuleName} error"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            var tickerContext = new TickerContext(_binanceClient);

            var currentPrice = tickerContext.GetPriceValue(solbot.Strategy.AvailableStrategy);

            if (currentPrice.Success)
            {
                solbot.Communication.Price = new PriceMessage
                {
                    Current = currentPrice.Result
                };

                return true;
            }
            return false;
        }
    }
}