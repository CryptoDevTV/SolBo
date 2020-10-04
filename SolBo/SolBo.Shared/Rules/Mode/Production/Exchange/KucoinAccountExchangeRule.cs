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

                var quote = accountType.FirstOrDefault(q => q.Currency == solbot.Communication.Symbol.QuoteAsset);
                KucoinAccount basee = null;

                if(solbot.Communication.Symbol.BaseAsset.ToUpper() == "BSV")
                {
                    basee = accountType.FirstOrDefault(q => q.Currency == "BCHSV");
                }
                else
                {
                    basee = accountType.FirstOrDefault(q => q.Currency == solbot.Communication.Symbol.BaseAsset);
                }

                if (accountType.AnyAndNotNull())
                {
                    solbot.Communication.AvailableAsset = new AvailableAssetMessage
                    {
                        Quote = quote.Available,
                        Base = basee.Available
                    };

                    baseMsg = $"{solbot.Communication.Symbol.BaseAsset}:{basee.Available}";
                    quoteMsg = $"{solbot.Communication.Symbol.QuoteAsset}:{quote.Available}";

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