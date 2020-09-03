using Kucoin.Net.Interfaces;
using NLog;
using SolBo.Shared.Domain.Configs;
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
            throw new NotImplementedException();
        }
    }
}