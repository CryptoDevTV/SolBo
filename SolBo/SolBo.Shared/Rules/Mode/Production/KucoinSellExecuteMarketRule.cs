using Binance.Net;
using Kucoin.Net.Interfaces;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class KucoinSellExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;
        private readonly IKucoinClient _kucoinClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public KucoinSellExecuteMarketRule(
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

            if (solbot.Communication.Sell.IsReady)
            {
                var quantity = BinanceHelpers.ClampQuantity(solbot.Communication.Symbol.MinQuantity, solbot.Communication.Symbol.MaxQuantity, solbot.Communication.Symbol.BasePrecision, solbot.Communication.AvailableAsset.Base);

                var sellOrderResult = _kucoinClient.PlaceOrder(
                    solbot.Strategy.AvailableStrategy.Symbol,
                    Kucoin.Net.Objects.KucoinOrderSide.Sell,
                    Kucoin.Net.Objects.KucoinNewOrderType.Market,
                    quantity: quantity);

                if (!(sellOrderResult is null))
                {
                    result = sellOrderResult.Success;

                    if (sellOrderResult.Success)
                    {
                        Logger.Info(LogGenerator.TradeResultStart(sellOrderResult.Data.OrderId));

                        var order = _kucoinClient.GetOrder(sellOrderResult.Data.OrderId);

                        if (!(order is null))
                        {
                            if (order.Success)
                            {
                                Logger.Info(LogGenerator.TradeResultStart(order.Data.ClientOrderId));

                                if (order.Data.DealQuantity != 0)
                                {
                                    var price = (order.Data.Funds / order.Data.DealQuantity).ToKucoinRound();
                                    Logger.Info(LogGenerator.TradeResultKucoin(MarketOrder, order.Data, price));

                                    solbot.Actions.BoughtPrice = price;
                                }

                                Logger.Info(LogGenerator.TradeResultEndKucoin(order.Data.ClientOrderId));
                            }
                            else
                                Logger.Warn(order.Error.Message);
                        }

                        solbot.Actions.BoughtPrice = 0;

                        Logger.Info(LogGenerator.TradeResultEndKucoin(sellOrderResult.Data.OrderId));

                        _pushOverNotificationService.Send(
                            LogGenerator.NotificationTitle(EnvironmentType.PRODUCTION, MarketOrder, solbot.Strategy.AvailableStrategy.Symbol),
                            LogGenerator.NotificationMessage(
                                solbot.Communication.Average.Current,
                                solbot.Communication.Price.Current,
                                solbot.Communication.Sell.Change));
                    }
                    else
                        Logger.Warn(sellOrderResult.Error.Message);
                }
                else
                    Logger.Warn(sellOrderResult.Error.Message);
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