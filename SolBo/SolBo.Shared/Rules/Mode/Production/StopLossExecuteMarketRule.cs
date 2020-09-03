using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot.SpotData;
using CryptoExchange.Net.Objects;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System.Collections.Generic;
using System.Linq;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class StopLossExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.STOPLOSS;
        private readonly IBinanceClient _binanceClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public StopLossExecuteMarketRule(
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

            if (solbot.Communication.StopLoss.IsReady)
            {
                WebCallResult<BinancePlacedOrder> stopLossOrderResult = null;

                var quantity = BinanceHelpers.ClampQuantity(solbot.Communication.Symbol.MinQuantity, solbot.Communication.Symbol.MaxQuantity, solbot.Communication.Symbol.StepSize, solbot.Communication.AvailableAsset.Base);

                var minNotional = quantity * solbot.Communication.Price.Current;

                if (minNotional > solbot.Communication.Symbol.MinNotional)
                {
                    stopLossOrderResult = _binanceClient.PlaceOrder(
                        solbot.Strategy.AvailableStrategy.Symbol,
                        OrderSide.Sell,
                        OrderType.Market,
                        quantity: quantity);
                }
                else
                    message = "not enough";

                if (!(stopLossOrderResult is null))
                {
                    result = stopLossOrderResult.Success;

                    if (stopLossOrderResult.Success)
                    {
                        solbot.Actions.BoughtPrice = 0;
                        solbot.Actions.StopLossReached = true;

                        Logger.Info(LogGenerator.TradeResultStart(stopLossOrderResult.Data.OrderId));

                        var prices = new List<decimal>();
                        var quantityAll = new List<decimal>();
                        var commission = new List<decimal>();

                        if (stopLossOrderResult.Data.Fills.AnyAndNotNull())
                        {
                            foreach (var item in stopLossOrderResult.Data.Fills)
                            {
                                Logger.Info(LogGenerator.TradeResult(MarketOrder, item));
                                prices.Add(item.Price);
                                quantityAll.Add(item.Quantity);
                                commission.Add(item.Commission);
                            }
                        }

                        Logger.Info(LogGenerator.TradeResultEnd(stopLossOrderResult.Data.OrderId, prices.Average(), quantityAll.Sum(), commission.Sum()));

                        _pushOverNotificationService.Send(
                            LogGenerator.NotificationTitle(EnvironmentType.PRODUCTION, MarketOrder, solbot.Strategy.AvailableStrategy.Symbol),
                            LogGenerator.NotificationMessage(
                                solbot.Communication.Average.Current,
                                solbot.Communication.Price.Current,
                                solbot.Communication.StopLoss.Change));
                    }
                    else
                        Logger.Warn(stopLossOrderResult.Error.Message);
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