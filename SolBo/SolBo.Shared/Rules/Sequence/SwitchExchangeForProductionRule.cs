using Binance.Net.Interfaces;
using Kucoin.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Rules.Mode;
using SolBo.Shared.Services;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Sequence
{
    public class SwitchExchangeForProductionRule : ISequencedRule
    {
        public string SequenceName => "EXCHANGE-PRODUCTION";
        private readonly IMarketService _marketService;
        private readonly Exchange _exchange;
        private readonly IBinanceClient _binanceClient;
        private readonly IKucoinClient _kucoinClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public SwitchExchangeForProductionRule(
            IMarketService marketService,
            Exchange exchange,
            IBinanceClient binanceClient,
            IKucoinClient kucoinClient,
            IPushOverNotificationService pushOverNotificationService)
        {
            _marketService = marketService;
            _exchange = exchange;
            _binanceClient = binanceClient;
            _kucoinClient = kucoinClient;
            _pushOverNotificationService = pushOverNotificationService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();

            switch (_exchange.Type.Value)
            {
                case Domain.Enums.ExchangeType.Binance:
                    {
                        _rules.Add(new BinanceModeProductionRule(
                            _marketService,
                            _pushOverNotificationService,
                            _binanceClient));
                    }
                    break;
                case Domain.Enums.ExchangeType.KuCoin:
                    {
                        _rules.Add(new KucoinModeProductionRule(
                            _marketService,
                            _pushOverNotificationService,
                            _kucoinClient));
                    }
                    break;
            }

            foreach (var item in _rules)
            {
                var res = item.RuleExecuted(solbot);

                if (res.Success)
                {
                    result.Success = true;
                    result.Message = LogGenerator.SequenceSuccess(SequenceName, _exchange.Type.Value.GetDescription());
                }
                else
                {
                    result.Success = false;
                    result.Message = LogGenerator.SequenceError(SequenceName, res.Message);
                    break;
                }
            }

            return result;
        }
    }
}