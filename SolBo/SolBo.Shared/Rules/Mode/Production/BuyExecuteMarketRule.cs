using Binance.Net.Interfaces;
using Binance.Net.Objects;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class BuyExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly IBinanceClient _binanceClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public BuyExecuteMarketRule(
            IBinanceClient binanceClient,
            IPushOverNotificationService pushOverNotificationService)
        {
            _binanceClient = binanceClient;
            _pushOverNotificationService = pushOverNotificationService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = false;
            var message = string.Empty;

            if (solbot.Communication.Buy.IsReady)
            {
                var buyOrderResult = _binanceClient.PlaceOrder(
                    solbot.Strategy.AvailableStrategy.Symbol,
                    OrderSide.Buy,
                    OrderType.Market,
                    quoteOrderQuantity: solbot.Communication.Buy.AvailableFund);

                if(!(buyOrderResult is null))
                {
                    result = buyOrderResult.Success;

                    if (buyOrderResult.Success)
                    {
                        solbot.Actions.Bought = 1;

                        Logger.Info(LogGenerator.TradeResultStart(buyOrderResult.Data.OrderId));

                        if (buyOrderResult.Data.Fills.AnyAndNotNull())
                        {
                            foreach (var item in buyOrderResult.Data.Fills)
                            {
                                Logger.Info(LogGenerator.TradeResult(item));
                            }
                        }

                        Logger.Info(LogGenerator.TradeResultEnd(buyOrderResult.Data.OrderId));

                        _pushOverNotificationService.Send(
                            LogGenerator.NotificationTitle(WorkingType.PRODUCTION, MarketOrder),
                            LogGenerator.NotificationMessage(
                                solbot.Communication.Average.Current,
                                solbot.Communication.Price.Current,
                                solbot.Communication.Buy.Change));
                    }
                    else
                        Logger.Warn(buyOrderResult.Error.Message);
                }
            }

            return new MarketRuleResult()
            {
                Success = result,
                Message = result
                    ? LogGenerator.OrderMarketSuccess(MarketOrder)
                    : LogGenerator.OrderMarketError(MarketOrder, message)
            };
        }
    }
}