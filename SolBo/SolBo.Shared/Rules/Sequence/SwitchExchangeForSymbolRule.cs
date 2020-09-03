using Binance.Net.Interfaces;
using Kucoin.Net.Interfaces;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Sequence
{
    public class SwitchExchangeForSymbolRule : ISequencedRule
    {
        public string SequenceName => "EXCHANGE-SYMBOL";
        private readonly Exchange _exchange;
        private readonly IBinanceClient _binanceClient;
        private readonly IKucoinClient _kucoinClient;
        private readonly IBinanceTickerService _binanceTickerService;
        private readonly IKucoinTickerService _kucoinTickerService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public SwitchExchangeForSymbolRule(
            Exchange exchange,
            IBinanceClient binanceClient,
            IKucoinClient kucoinClient,
            IBinanceTickerService binanceTickerService,
            IKucoinTickerService kucoinTickerService)
        {
            _exchange = exchange;
            _binanceClient = binanceClient;
            _kucoinClient = kucoinClient;
            _binanceTickerService = binanceTickerService;
            _kucoinTickerService = kucoinTickerService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();

            if (_exchange.Type.HasValue)
            {
                switch (_exchange.Type.Value)
                {
                    case Domain.Enums.ExchangeType.Binance:
                        {
                            _rules.Add(new BinanceSymbolSequenceRule(_binanceClient));
                            _rules.Add(new BinanceGetPriceSequenceRule(_binanceTickerService));
                        }
                        break;
                    case Domain.Enums.ExchangeType.KuCoin:
                        {
                            _rules.Add(new KucoinSymbolSequenceRule(_kucoinClient));
                            _rules.Add(new KucoinGetPriceSequenceRule(_kucoinTickerService));
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
            }
            else
            {
                result.Success = false;
                result.Message = "No exchange type provided";
            }
            return result;
        }
    }
}