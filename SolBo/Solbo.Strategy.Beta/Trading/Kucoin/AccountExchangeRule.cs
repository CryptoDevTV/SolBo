using Kucoin.Net.Interfaces;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solbo.Strategy.Beta.Trading.Kucoin
{
    public class AccountExchangeRule : IBetaRule
    {
        private readonly IKucoinClient _kucoinClient;
        public AccountExchangeRule(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var accountInfo = _kucoinClient.GetAccounts();

            if (accountInfo.Success)
            {

            }

            throw new NotImplementedException();
        }
    }
}