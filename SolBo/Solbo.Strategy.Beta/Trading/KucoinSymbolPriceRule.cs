using Kucoin.Net.Interfaces;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Beta.Trading
{
    internal class KucoinSymbolPriceRule : IBetaRules
    {
        private readonly IKucoinClient _kucoinClient;
        public KucoinSymbolPriceRule(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var priceResponse = _kucoinClient.GetTicker(strategyModel.Symbol);

                if (priceResponse.Success)
                {
                    //priceResponse.Data.LastTradePrice.GetValueOrDefault()
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