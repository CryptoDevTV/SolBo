using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Mode
{
    public class ModeProductionRule : IModeRule
    {
        public string ModeName => "PRODUCTION MODE (not working now)";
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        private readonly IMarketService _marketService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public ModeProductionRule(IMarketService marketService)
        {
            _marketService = marketService;
        }

        public IRuleResult RuleExecuted(Solbot solbot)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(solbot.Exchange.ApiKey, solbot.Exchange.ApiSecret)
            });

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