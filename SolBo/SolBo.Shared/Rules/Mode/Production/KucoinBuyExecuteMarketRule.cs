using Kucoin.Net.Interfaces;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class KucoinBuyExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly IKucoinClient _kucoinClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public KucoinBuyExecuteMarketRule(
            IKucoinClient kucoinClient,
            IPushOverNotificationService pushOverNotificationService)
        {
            _kucoinClient = kucoinClient;
            _pushOverNotificationService = pushOverNotificationService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = false;
            var message = string.Empty;

            if (solbot.Communication.Buy.IsReady)
            {
                var funds = solbot.Communication.Buy.AvailableFund;

                var buyOrderResult = _kucoinClient.PlaceOrder(
                    solbot.Strategy.AvailableStrategy.Symbol,
                    Kucoin.Net.Objects.KucoinOrderSide.Buy,
                    Kucoin.Net.Objects.KucoinNewOrderType.Market,
                    funds: funds);

                if (!(buyOrderResult is null))
                {
                    result = buyOrderResult.Success;

                    if (buyOrderResult.Success)
                    {
                        var order = _kucoinClient.GetOrder(buyOrderResult.Data.OrderId);

                        if (!(order is null))
                        {
                            if (order.Success)
                            {
                                Logger.Info(LogGenerator.TradeResultStart(order.Data.ClientOrderId));

                                if (order.Data.DealQuantity != 0)
                                {
                                    var price = ((order.Data.Funds ?? 0) / order.Data.DealQuantity).ToKucoinRound();
                                    Logger.Info(LogGenerator.TradeResultKucoin(MarketOrder, order.Data, price));

                                    solbot.Actions.BoughtPrice = price;
                                }

                                Logger.Info(LogGenerator.TradeResultEndKucoin(order.Data.ClientOrderId));
                            }
                            else
                                Logger.Warn(order.Error.Message);
                        }

                        _pushOverNotificationService.Send(
                            LogGenerator.NotificationTitle(EnvironmentType.PRODUCTION, MarketOrder, solbot.Strategy.AvailableStrategy.Symbol),
                            LogGenerator.NotificationMessage(
                                solbot.Communication.Average.Current,
                                solbot.Communication.Price.Current,
                                solbot.Communication.Buy.Change));
                    }
                    else
                        Logger.Warn(buyOrderResult.Error.Message);
                }
                else
                    Logger.Warn(buyOrderResult.Error.Message);
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