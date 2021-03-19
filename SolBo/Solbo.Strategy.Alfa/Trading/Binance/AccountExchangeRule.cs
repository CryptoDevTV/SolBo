using Binance.Net.Interfaces;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Linq;

namespace Solbo.Strategy.Alfa.Trading.Binance
{
    public class AccountExchangeRule : IAlfaRule
    {
        private readonly IBinanceClient _binanceClient;
        public AccountExchangeRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var accountInfo = _binanceClient.General.GetAccountInfo();

                if (accountInfo.Success)
                {
                    strategyModel.Communication.BaseAsset = accountInfo.Data.Balances.FirstOrDefault(e => e.Asset == strategyModel.Communication.BinanceSymbol.BaseAsset).Free;
                    strategyModel.Communication.QuoteAsset = accountInfo.Data.Balances.FirstOrDefault(e => e.Asset == strategyModel.Communication.BinanceSymbol.QuoteAsset).Free;
                }
                else
                {
                    errors += accountInfo.Error?.Message;
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