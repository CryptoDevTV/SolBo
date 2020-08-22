using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Rules.Order;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class BuyPriceMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly ICollection<IOrderRule> _rules = new HashSet<IOrderRule>();
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            _rules.Add(new AvailableQuoteAssetRule());
            _rules.Add(new AvailableEnoughQuoteAssetRule());
            _rules.Add(new BuyPriceReachedRule());
            _rules.Add(new BoughtPriceBeforeBuyRule());

            var result = true;

            foreach (var item in _rules)
            {
                var resultOrderStep = item.RuleExecuted(solbot);

                if(resultOrderStep.Success)
                    Logger.Info($"{resultOrderStep.Message}");
                else
                {
                    result = false;
                    Logger.Warn($"{resultOrderStep.Message}");
                }
            }

            solbot.Communication.Buy.IsReady = result;

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.PriceMarketSuccess(MarketOrder)
                    : LogGenerator.PriceMarketError(MarketOrder)
            };
        }
    }
}