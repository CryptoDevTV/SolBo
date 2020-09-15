using Kucoin.Net.Interfaces;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Rules.Mode.Production;
using SolBo.Shared.Rules.Mode.Production.Exchange;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Mode
{
    public class KucoinModeProductionRule : IModeRule
    {
        public string ModeName => "KUCOIN PRODUCTION MODE";
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        private readonly IMarketService _marketService;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        private readonly IKucoinClient _kucoinClient;
        public KucoinModeProductionRule(
            IMarketService marketService,
            IPushOverNotificationService pushOverNotificationService,
            IKucoinClient kucoinClient)
        {
            _marketService = marketService;
            _pushOverNotificationService = pushOverNotificationService;
            _kucoinClient = kucoinClient;
        }

        public IRuleResult RuleExecuted(Solbot solbot)
        {
            _rules.Add(new KucoinAccountExchangeRule(_kucoinClient));

            if (solbot.Strategy.AvailableStrategy.IsStopLossOn && solbot.Actions.BoughtBefore)
            {
                _rules.Add(new StopLossStepMarketRule(_marketService));
                _rules.Add(new StopLossPriceMarketRule());
                _rules.Add(new KucoinStopLossExecuteMarketRule(_kucoinClient, _pushOverNotificationService));
            }

            if (solbot.Actions.BoughtBefore)
            {
                _rules.Add(new SellStepMarketRule(_marketService));
                _rules.Add(new SellPriceMarketRule());
                //_rules.Add(new KucoinSellExecuteMarketRule(_kucoinClient, _pushOverNotificationService));
            }

            if (solbot.Actions.SellBefore)
            {
                _rules.Add(new BuyStepMarketRule(_marketService, true));
                _rules.Add(new BuyPriceMarketRule());
                _rules.Add(new KucoinBuyExecuteMarketRule(_kucoinClient, _pushOverNotificationService));
            }

            Logger.Info(LogGenerator.ModeStart(ModeName));

            foreach (var item in _rules)
            {
                var result = item.RuleExecuted(solbot);

                Logger.Info($"{result.Message}");
            }

            Logger.Info(LogGenerator.ModeEnd(ModeName));

            return new ModeRuleResult
            {
                Message = LogGenerator.ModeExecuted(ModeName),
                Success = true
            };
        }
    }
}