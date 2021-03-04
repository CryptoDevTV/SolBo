using Kucoin.Net.Interfaces;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Linq;

namespace Solbo.Strategy.Beta.Trading
{
    internal class KucoinSymbolRule : IBetaRules
    {
        private readonly IKucoinClient _kucoinClient;
        public KucoinSymbolRule(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var exchangeInfo = _kucoinClient.GetSymbols();
                if (exchangeInfo.Success)
                {
                    var symbol = exchangeInfo
                        .Data
                        .FirstOrDefault(e => e.Symbol == strategyModel.Symbol);

                    if (!(symbol is null) && symbol.EnableTrading)
                    {
                        strategyModel.Communication.KucoinSymbol = symbol;
                    }
                    else
                    {
                        errors += $"Something went wrong while fetching symbol ({strategyModel.Symbol}) data from Kucoin";
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