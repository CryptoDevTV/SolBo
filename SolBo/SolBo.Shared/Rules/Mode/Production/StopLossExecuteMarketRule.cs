using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptoExchange.Net.Objects;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class StopLossExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.STOPLOSS;
        private readonly IMarketService _marketService;
        private readonly IBinanceClient _binanceClient;
        public StopLossExecuteMarketRule(
            IMarketService marketService,
            IBinanceClient binanceClient)
        {
            _marketService = marketService;
            _binanceClient = binanceClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = false;

            if (solbot.Communication.StopLoss.IsReady)
            {
                WebCallResult<BinancePlacedOrder> stopLossOrderResult = null;

                var quantity = BinanceHelpers.ClampQuantity(solbot.Communication.Symbol.MinQuantity, solbot.Communication.Symbol.MaxQuantity, solbot.Communication.Symbol.StepSize, solbot.Communication.AvailableAsset.Base);

                if (solbot.Strategy.AvailableStrategy.StopLossType == StopLossType.MARKETSELL)
                {
                    var minNotional = quantity * solbot.Communication.Price.Current;

                    if (minNotional > solbot.Communication.Symbol.MinNotional)
                    {
                        stopLossOrderResult = _binanceClient.PlaceOrder(
                            solbot.Strategy.AvailableStrategy.Symbol,
                            OrderSide.Sell,
                            OrderType.Market,
                            quantity: quantity);
                    }
                }
                else
                {
                    var stopLossPrice = BinanceHelpers.ClampPrice(solbot.Communication.Symbol.MinPrice, solbot.Communication.Symbol.MaxPrice, solbot.Communication.Price.Current);

                    var minNotional = quantity * stopLossPrice;

                    if (minNotional > solbot.Communication.Symbol.MinNotional)
                    {
                        stopLossOrderResult = _binanceClient.PlaceOrder(
                            solbot.Strategy.AvailableStrategy.Symbol,
                            OrderSide.Sell,
                            OrderType.StopLossLimit,
                            quantity: quantity,
                            stopPrice: BinanceHelpers.FloorPrice(solbot.Communication.Symbol.TickSize, stopLossPrice),
                            price: BinanceHelpers.FloorPrice(solbot.Communication.Symbol.TickSize, stopLossPrice),
                            timeInForce: TimeInForce.GoodTillCancel);
                    }
                }

                if (!(stopLossOrderResult is null))
                {
                    result = stopLossOrderResult.Success;

                    if (stopLossOrderResult.Success)
                    {
                        Logger.Info(LogGenerator.StopLossResultStart(stopLossOrderResult.Data.OrderId));

                        if (stopLossOrderResult.Data.Fills.AnyAndNotNull())
                        {
                            foreach (var item in stopLossOrderResult.Data.Fills)
                            {
                                Logger.Info(LogGenerator.StopLossResult(item));
                            }
                        }

                        Logger.Info(LogGenerator.StopLossResultEnd(stopLossOrderResult.Data.OrderId));
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
                    : LogGenerator.OrderMarketError(MarketOrder)
            };
        }
    }
}