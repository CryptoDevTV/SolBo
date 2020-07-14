using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Rules.Mode.Production;
using SolBo.Shared.Rules.Mode.Production.Exchange;
using SolBo.Shared.Services;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Mode
{
    public class ModeProductionRule : IModeRule
    {
        public string ModeName => "PRODUCTION MODE";
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        private readonly IMarketService _marketService;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public ModeProductionRule(
            IMarketService marketService,
            IPushOverNotificationService pushOverNotificationService)
        {
            _marketService = marketService;
            _pushOverNotificationService = pushOverNotificationService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(solbot.Exchange.ApiKey, solbot.Exchange.ApiSecret)
            });

            using (var binanceClient = new BinanceClient())
            {
                _rules.Add(new AccountExchangeRule(binanceClient));

                _rules.Add(new StopLossStepMarketRule(_marketService));
                _rules.Add(new StopLossPriceMarketRule());
                _rules.Add(new StopLossExecuteMarketRule(binanceClient, _pushOverNotificationService));

                _rules.Add(new SellStepMarketRule(_marketService));
                _rules.Add(new SellPriceMarketRule());
                _rules.Add(new SellExecuteMarketRule(binanceClient, _pushOverNotificationService));

                _rules.Add(new BuyStepMarketRule(_marketService, true));
                _rules.Add(new BuyPriceMarketRule());
                _rules.Add(new BuyExecuteMarketRule(binanceClient, _pushOverNotificationService));

                Logger.Info(LogGenerator.ModeStart(ModeName));

                foreach (var item in _rules)
                {
                    var result = item.RuleExecuted(solbot);

                    Logger.Info($"{result.Message}");
                }

                Logger.Info(LogGenerator.ModeEnd(ModeName));
            }

            return new ModeRuleResult
            {
                Message = LogGenerator.ModeExecuted(ModeName),
                Success = true
            };
        }
    }
}