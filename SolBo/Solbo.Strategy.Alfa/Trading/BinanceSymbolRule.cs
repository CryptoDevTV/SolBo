using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Linq;

namespace Solbo.Strategy.Alfa.Trading
{
    internal class BinanceSymbolRule : IAlfaRule
    {
        private readonly IBinanceClient _binanceClient;
        public BinanceSymbolRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var exchangeInfo = _binanceClient.Spot.System.GetExchangeInfo();

                if (exchangeInfo.Success)
                {
                    var symbol = exchangeInfo
                        .Data
                        .Symbols
                        .FirstOrDefault(e => e.Name == strategyModel.Symbol);

                    if (!(symbol is null) && symbol.Status == SymbolStatus.Trading)
                    {
                        //symbol.BaseAsset
                    }
                    else
                    {
                        errors += $"Something went wrong while fetching symbol ({strategyModel.Symbol}) data from Binance";
                    }
                }
            }
            catch (Exception ex)
            {
                errors += ex.GetFullMessage();
            }
            return new RuleResult(errors);
        }
    }
}