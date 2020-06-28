using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptoExchange.Net.Objects;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class StopLossExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.STOPLOSS;
        private readonly IBinanceClient _binanceClient;
        public StopLossExecuteMarketRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = false;
            var message = string.Empty;

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
                    else
                        message = "not enough";
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
                    else
                        message = "not enough";
                }

                if (!(stopLossOrderResult is null))
                {
                    result = stopLossOrderResult.Success;

                    if (stopLossOrderResult.Success)
                    {
                        Logger.Info(LogGenerator.TradeResultStart(stopLossOrderResult.Data.OrderId));

                        if (stopLossOrderResult.Data.Fills.AnyAndNotNull())
                        {
                            foreach (var item in stopLossOrderResult.Data.Fills)
                            {
                                Logger.Info(LogGenerator.TradeResult(item));
                            }
                        }

                        Logger.Info(LogGenerator.TradeResultEnd(stopLossOrderResult.Data.OrderId));
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