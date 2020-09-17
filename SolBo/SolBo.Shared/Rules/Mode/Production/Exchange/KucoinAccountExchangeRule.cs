using Kucoin.Net.Interfaces;
using Kucoin.Net.Objects;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Messages.Rules;
using System.Linq;

namespace SolBo.Shared.Rules.Mode.Production.Exchange
{
    public class KucoinAccountExchangeRule : IExchangeRule
    {
        private readonly IKucoinClient _kucoinClient;
        public KucoinAccountExchangeRule(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var accountInfo = _kucoinClient.GetAccounts();

            var result = false;
            var baseMsg = string.Empty;
            var quoteMsg = string.Empty;

            if (accountInfo.Success)
            {
                var accountType = accountInfo.Data.Where(a => a.Type == KucoinAccountType.Trade).ToList();

                var q = accountType.FirstOrDefault(q => q.Currency == solbot.Communication.Symbol.QuoteAsset);
                accountType.Remove(q); // because of BCHSV <> BSV
                var b = accountType.FirstOrDefault()?.Available;

                if (accountType.AnyAndNotNull())
                {
                    solbot.Communication.AvailableAsset = new AvailableAssetMessage
                    {
                        Quote = q.Available,
                        Base = b ?? 0m
                    };

                    baseMsg = $"{solbot.Communication.Symbol.BaseAsset}:{b ?? 0m}";
                    quoteMsg = $"{solbot.Communication.Symbol.QuoteAsset}:{q.Available}";

                    result = true;
                }
            }

            return new ExchangeRuleResult
            {
                Success = result,
                Message = LogGenerator.ExchangeLog(baseMsg, quoteMsg, accountInfo.Error?.Message)
            };
        }
    }
}