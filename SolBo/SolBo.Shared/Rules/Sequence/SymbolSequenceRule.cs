using Binance.Net.Interfaces;
using Binance.Net.Objects;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using System;
using System.Linq;

namespace SolBo.Shared.Rules.Sequence
{
    public class SymbolSequenceRule : ISequencedRule
    {
        public string SequenceName => "SYMBOL";
        private readonly IBinanceClient _binanceClient;
        public SymbolSequenceRule(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                var exchangeInfo = _binanceClient.GetExchangeInfo();

                if (exchangeInfo.Success)
                {
                    var symbol = exchangeInfo
                        .Data
                        .Symbols
                        .FirstOrDefault(e => e.Name == solbot.Strategy.AvailableStrategy.Symbol);
                    
                    if (!(symbol is null) && symbol.Status == SymbolStatus.Trading)
                    {
                        solbot.Communication = new Communication
                        {
                            Symbol = new SymbolMessage
                            {
                                BaseAsset = symbol.BaseAsset,
                                QuoteAsset = symbol.QuoteAsset,
                                QuoteAssetPrecision = symbol.QuoteAssetPrecision,
                                MinNotional = symbol.MinNotionalFilter.MinNotional,
                                StepSize = symbol.LotSizeFilter.StepSize,
                                MaxQuantity = symbol.LotSizeFilter.MaxQuantity,
                                MinQuantity = symbol.LotSizeFilter.MinQuantity,
                                TickSize = symbol.PriceFilter.TickSize,
                                MaxPrice = symbol.PriceFilter.MaxPrice,
                                MinPrice = symbol.PriceFilter.MinPrice
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