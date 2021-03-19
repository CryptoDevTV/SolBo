using Kucoin.Net.Interfaces;
using Kucoin.Net.Objects;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Linq;

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
            var errors = string.Empty;
            try
            {
                var accountInfo = _kucoinClient.GetAccounts();

                if (accountInfo.Success)
                {
                    var accountType = accountInfo.Data.Where(a => a.Type == KucoinAccountType.Trade).ToList();

                    var quote = accountType.FirstOrDefault(q => q.Currency == strategyModel.Communication.KucoinSymbol.QuoteCurrency);
                    KucoinAccount baseAccount = null;

                    if (strategyModel.Communication.KucoinSymbol.BaseCurrency.ToUpper() == "BSV")
                    {
                        baseAccount = accountType.FirstOrDefault(q => q.Currency == "BCHSV");
                    }
                    else
                    {
                        baseAccount = accountType.FirstOrDefault(q => q.Currency == strategyModel.Communication.KucoinSymbol.BaseCurrency);
                    }

                    var baseAvailable = baseAccount != null ? baseAccount.Available : 0m;

                    strategyModel.Communication.BaseAsset = baseAvailable;
                    strategyModel.Communication.QuoteAsset = quote.Available;
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