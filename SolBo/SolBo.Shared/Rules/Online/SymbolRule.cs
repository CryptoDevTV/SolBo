using Binance.Net.Interfaces;
using Binance.Net.Objects;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Messages.Rules;
using System;
using System.Linq;

namespace SolBo.Shared.Rules.Online
{
    public class SymbolRule : IRule
    {
        private readonly IBinanceClient _binanceClient;
        public SymbolRule(IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public string RuleName => "SYMBOL VALIDATION";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Symbol: {solbot.Strategy.AvailableStrategy.Symbol}"
                    : $"{RuleName} error. {Message}"
            };
        }

        public bool RulePassed(Solbot solbot)
        {
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

                        return true;
                    }

                    Message = $"Symbol: {solbot.Strategy.AvailableStrategy.Symbol} not exist on {solbot.Exchange.Name}.";

                    return false;
                }

                Message = exchangeInfo.Error.Message;

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