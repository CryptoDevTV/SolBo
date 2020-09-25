using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
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
    public class BinanceSellExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;
        private readonly IBinanceClient _binanceClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public BinanceSellExecuteMarketRule(
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

            if (solbot.Communication.Sell.IsReady)
            {
                var quantity = BinanceHelpers.ClampQuantity(solbot.Communication.Symbol.MinQuantity, solbot.Communication.Symbol.MaxQuantity, solbot.Communication.Symbol.StepSize, solbot.Communication.AvailableAsset.Base);

                var minNotional = quantity * solbot.Communication.Price.Current;

                if (minNotional > solbot.Communication.Symbol.MinNotional)
                {
                    var sellOrderResult = _binanceClient.Spot.Order.PlaceOrder(
                        solbot.Strategy.AvailableStrategy.Symbol,
                        OrderSide.Sell,
                        OrderType.Market,
                        quantity: quantity);

                    if (!(sellOrderResult is null))
                    {
                        result = sellOrderResult.Success;

                        if (sellOrderResult.Success)
                        {
                            solbot.Actions.BoughtPrice = 0;

                            Logger.Info(LogGenerator.TradeResultStart($"{sellOrderResult.Data.OrderId}"));

                            var prices = new List<decimal>();
                            var quantityAll = new List<decimal>();
                            var commission = new List<decimal>();

                            if (sellOrderResult.Data.Fills.AnyAndNotNull())
                            {
                                foreach (var item in sellOrderResult.Data.Fills)
                                {
                                    Logger.Info(LogGenerator.TradeResult(MarketOrder, item));
                                    prices.Add(item.Price);
                                    quantityAll.Add(item.Quantity);
                                    commission.Add(item.Commission);
                                }
                            }

                            Logger.Info(LogGenerator.TradeResultEnd($"{sellOrderResult.Data.OrderId}", prices.Average(), quantityAll.Sum(), commission.Sum()));

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
                }
                else
                    message = "not enough";
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