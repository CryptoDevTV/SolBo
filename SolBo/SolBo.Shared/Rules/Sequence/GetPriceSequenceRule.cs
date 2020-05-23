using Binance.Net.Interfaces;
using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class GetPriceSequenceRule : ISequencedRule
    {
        public string SequenceName => "PRICE";
        private readonly IBinanceClient _binanceClient;
        public GetPriceSequenceRule(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
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
                    result.Success = true;
                    result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{currentPrice.Result}");
                }
                else
                {
                    result.Success = false;
                    result.Message = LogGenerator.SequenceError(SequenceName, currentPrice.Message);
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = LogGenerator.SequenceException(SequenceName, e);
            }
            return result;
        }
    }
}