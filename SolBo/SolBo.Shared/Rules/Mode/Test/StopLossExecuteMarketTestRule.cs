using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Test
{
    public class StopLossExecuteMarketTestRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.STOPLOSS;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public StopLossExecuteMarketTestRule(
            IPushOverNotificationService pushOverNotificationService)
        {
            _pushOverNotificationService = pushOverNotificationService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = solbot.Communication.StopLoss.PriceReached && solbot.Actions.BoughtPrice > 0;

            if (result)
            {
                solbot.Actions.BoughtPrice = 0;
                solbot.Actions.StopLossReached = true;

                _pushOverNotificationService.Send(
                    LogGenerator.NotificationTitle(WorkingType.TEST, MarketOrder),
                    LogGenerator.NotificationMessage(
                        solbot.Communication.Average.Current,
                        solbot.Communication.Price.Current,
                        solbot.Communication.StopLoss.Change));
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.ExecuteMarketSuccess(MarketOrder, solbot.Communication.StopLoss.PriceReached, solbot.Actions.BoughtPrice)
                    : LogGenerator.ExecuteMarketError(MarketOrder, solbot.Communication.StopLoss.PriceReached, solbot.Actions.BoughtPrice)
            };
        }
    }
}