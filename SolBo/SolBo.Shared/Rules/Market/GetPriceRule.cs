using Binance.Net.Interfaces;
using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Messages.Rules;
using System;

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
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Price: {solbot.Communication.Price.Current}"
                    : $"{RuleName} error. {Message}"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            try
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

                Message = currentPrice.Message;

                return false;
            }
            catch (Exception e)
            {
                Message = e.GetFullMessage();

                return false;
            }
        }
    }
}