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
    public class BinanceBuyExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly IBinanceClient _binanceClient;
        private readonly IPushOverNotificationService _pushOverNotificationService;
        public BinanceBuyExecuteMarketRule(
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

                if (!(buyOrderResult is null))
                {
                    result = buyOrderResult.Success;

                    if (buyOrderResult.Success)
                    {
                        Logger.Info(LogGenerator.TradeResultStart(buyOrderResult.Data.OrderId));

                        var prices = new List<decimal>();
                        var quantity = new List<decimal>();
                        var commission = new List<decimal>();

                        if (buyOrderResult.Data.Fills.AnyAndNotNull())
                        {
                            foreach (var item in buyOrderResult.Data.Fills)
                            {
                                Logger.Info(LogGenerator.TradeResult(MarketOrder, item));
                                prices.Add(item.Price);
                                quantity.Add(item.Quantity);
                                commission.Add(item.Commission);
                            }
                        }

                        solbot.Actions.BoughtPrice = prices.Average();

                        Logger.Info(LogGenerator.TradeResultEnd(buyOrderResult.Data.OrderId, prices.Average(), quantity.Sum(), commission.Sum()));

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