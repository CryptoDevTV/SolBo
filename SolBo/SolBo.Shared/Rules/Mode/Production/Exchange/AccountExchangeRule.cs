using Binance.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using System.Linq;

namespace SolBo.Shared.Rules.Mode.Production.Exchange
{
    public class AccountExchangeRule : IExchangeRule
    {
        private readonly IBinanceClient _binanceClient;
        public AccountExchangeRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var accountInfo = _binanceClient.GetAccountInfo();

            var result = false;
            var baseMsg = string.Empty;
            var quoteMsg = string.Empty;

            if(accountInfo.Success)
            {
                var availableBase = accountInfo.Data.Balances.FirstOrDefault(e => e.Asset == solbot.Communication.Symbol.BaseAsset).Free;
                var availableQuote = accountInfo.Data.Balances.FirstOrDefault(e => e.Asset == solbot.Communication.Symbol.QuoteAsset).Free;

                solbot.Communication.AvailableAsset = new AvailableAssetMessage
                {
                    Base = availableBase,
                    Quote = availableQuote
                };

                baseMsg = $"{solbot.Communication.Symbol.BaseAsset}:{availableBase}";
                quoteMsg = $"{solbot.Communication.Symbol.QuoteAsset}:{availableQuote}";

                result = true;
            }

            return new ExchangeRuleResult
            {
                Success = result,
                Message = LogGenerator.ExchangeLog(baseMsg, quoteMsg, accountInfo.Error?.Message)
            };
        }
    }
}