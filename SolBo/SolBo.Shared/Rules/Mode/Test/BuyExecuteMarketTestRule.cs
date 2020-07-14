using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class BuyExecuteMarketTestRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public BuyExecuteMarketTestRule(
            IPushOverNotificationService pushOverNotificationService)
        {
            _pushOverNotificationService = pushOverNotificationService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.Buy.PriceReached && solbot.Actions.BoughtPrice == 0;

            if (result)
            {
                solbot.Actions.BoughtPrice = solbot.Communication.Price.Current;
                result = true;

                _pushOverNotificationService.Send(
                    LogGenerator.NotificationTitle(WorkingType.TEST, MarketOrder, solbot.Strategy.AvailableStrategy.Symbol),
                    LogGenerator.NotificationMessage(
                        solbot.Communication.Average.Current,
                        solbot.Communication.Price.Current,
                        solbot.Communication.Buy.Change));
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.ExecuteMarketSuccess(MarketOrder, solbot.Communication.Buy.PriceReached, solbot.Actions.BoughtPrice)
                    : LogGenerator.ExecuteMarketError(MarketOrder, solbot.Communication.Buy.PriceReached, solbot.Actions.BoughtPrice)
            };
        }
    }
}