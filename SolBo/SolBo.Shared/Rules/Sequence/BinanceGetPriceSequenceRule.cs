using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class BinanceGetPriceSequenceRule : ISequencedRule
    {
        public string SequenceName => "BINANCE-PRICE";
        private readonly IBinanceTickerService _tickerPriceService;
        public BinanceGetPriceSequenceRule(IBinanceTickerService tickerPriceService)
        {
            _tickerPriceService = tickerPriceService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                var currentPrice = _tickerPriceService.GetPriceValue(solbot.Strategy.AvailableStrategy);

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