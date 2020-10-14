using Kucoin.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using System;
using System.Linq;

namespace SolBo.Shared.Rules.Sequence
{
    public class KucoinSymbolSequenceRule : ISequencedRule
    {
        public string SequenceName => "SYMBOL-KUCOIN";
        private readonly IKucoinClient _kucoinClient;
        public KucoinSymbolSequenceRule(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                var exchangeInfo = _kucoinClient.GetSymbols();

                if(exchangeInfo.Success)
                {
                    var symbol = exchangeInfo
                        .Data
                        .FirstOrDefault(e => e.Symbol == solbot.Strategy.AvailableStrategy.Symbol);

                    if (!(symbol is null) && symbol.EnableTrading)
                    {
                        solbot.Communication = new Communication
                        {
                            Symbol = new SymbolMessage
                            {
                                BaseAsset = symbol.BaseCurrency,
                                QuoteAsset = symbol.QuoteCurrency,
                                BasePrecision = symbol.BaseIncrement,
                                QuotePrecision = symbol.QuoteIncrement,
                                BaseAssetPrecision = BitConverter.GetBytes(decimal.GetBits(symbol.BaseIncrement)[3])[2],
                                QuoteAssetPrecision = BitConverter.GetBytes(decimal.GetBits(symbol.QuoteIncrement)[3])[2],
                                StepSize = symbol.PriceIncrement,
                                MaxQuantity = symbol.QuoteMaxSize,
                                MinQuantity = symbol.QuoteMinSize
                            }
                        };
                        result.Success = true;
                        result.Message = LogGenerator.SequenceSuccess(SequenceName, solbot.Strategy.AvailableStrategy.Symbol);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = LogGenerator.SequenceError(SequenceName, solbot.Strategy.AvailableStrategy.Symbol);
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = LogGenerator.SequenceError(SequenceName, exchangeInfo.Error.Message);
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