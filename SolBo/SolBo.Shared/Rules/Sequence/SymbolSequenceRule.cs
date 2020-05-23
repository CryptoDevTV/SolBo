using Binance.Net.Interfaces;
using Binance.Net.Objects;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Messages.Rules;
using System;
using System.Linq;

namespace SolBo.Shared.Rules.Sequence
{
    public class SymbolSequenceRule : ISequencedRule
    {
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
                                QuoteAsset = symbol.QuoteAsset
                            }
                        };
                    }
                    else
                        result.Message = $"Symbol: {solbot.Strategy.AvailableStrategy.Symbol} not exist on {solbot.Exchange.Name}.";
                }
                else
                    result.Message = exchangeInfo.Error.Message;
            }
            catch (Exception e)
            {
                result.Message = e.GetFullMessage();
            }
            return result;
        }
    }
}