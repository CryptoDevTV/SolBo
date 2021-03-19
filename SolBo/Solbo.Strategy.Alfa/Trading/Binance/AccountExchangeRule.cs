using Binance.Net.Interfaces;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Collections.Generic;
using System.Text;

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
            var accountInfo = _binanceClient.General.GetAccountInfo();

            if (accountInfo.Success)
            {

            }

            throw new NotImplementedException();
        }
    }
}