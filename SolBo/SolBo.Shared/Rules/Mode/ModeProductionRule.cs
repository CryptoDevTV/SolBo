using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Mode
{
    public class ModeProductionRule : IRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        private readonly IMarketService _marketService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public ModeProductionRule(IMarketService marketService)
        {
            _marketService = marketService;

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("apikey", "apisecret")
            });
        }

        public string RuleName => "PRODUCTION MODE";

        public ResultRule ExecutedRule(Solbot solbot)
        {
            Logger.Info($"{RuleName} START");

            foreach (var item in _rules)
            {
                var result = item.ExecutedRule(solbot);

                Logger.Info($"{result.Message}");
            }

            Logger.Info($"{RuleName} END");

            return new ResultRule
            {
                Success = RulePassed(solbot),
                Message = "{RuleName} EXECUTED"
            };
        }

        public bool RulePassed(Solbot solbot)
            => true;
    }
}