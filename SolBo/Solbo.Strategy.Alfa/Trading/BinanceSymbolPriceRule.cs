using Binance.Net.Interfaces;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Trading
{
    internal class BinanceSymbolPriceRule : IAlfaRule
    {
        private readonly IBinanceClient _binanceClient;
        public BinanceSymbolPriceRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }

        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var priceResponse = _binanceClient.Spot.Market.GetPrice(strategyModel.Symbol);
                if (priceResponse.Success)
                {
                    //priceResponse.Data.Price
                }
                else
                {
                    errors += priceResponse.Error.Message;
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