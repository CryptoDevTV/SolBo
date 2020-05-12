using Binance.Net;
using Binance.Net.Objects;
using NLog;
using Quartz;
using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolBo.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class BuyDeepSellHighJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        private readonly IStorageService _storageService;
        private readonly IMarketService _marketService;

        public BuyDeepSellHighJob(
            IStorageService storageService,
            IMarketService marketService)
        {
            _storageService = storageService;
            _marketService = marketService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;

            var availableStrategy = strategy.Available.FirstOrDefault(s => s.Id == strategy.ActiveId);

            if (!(availableStrategy is null))
            {
                _storageService.SetPath(Path.Combine(availableStrategy.StoragePath, $"{availableStrategy.Symbol}.txt"));

                using (var client = new BinanceClient())
                {
                    var tickerContext = new TickerContext(client);

                    var accountInfo = await client.GetAccountInfoAsync();

                    if (accountInfo.Success)
                    {
                        var exchangeInfo = await client.GetExchangeInfoAsync();

                        if (exchangeInfo.Success)
                        {
                            var symbol = exchangeInfo.Data.Symbols
                                .FirstOrDefault(e => e.Name == availableStrategy.Symbol);

                            if (!(symbol is null) && symbol.Status == SymbolStatus.Trading)
                            {
                                var baseAsset = symbol.BaseAsset;
                                var quoteAsset = symbol.QuoteAsset;

                                var currentPrice = await tickerContext.GetPriceValue(availableStrategy);

                                if (currentPrice.Success)
                                {
                                    var price = currentPrice.Result;

                                    var availableBase = accountInfo.Data.Balances.FirstOrDefault(e => e.Asset == baseAsset).Free;
                                    var availableQuote = accountInfo.Data.Balances.FirstOrDefault(e => e.Asset == quoteAsset).Free;

                                    Logger.Info(LogGenerator.CurrentPrice(availableStrategy, price));

                                    _storageService.SaveValue(price);

                                    var storedPriceAverage = AverageContext.Average(_storageService.GetValues(), 4, availableStrategy.Average);

                                    Logger.Info(LogGenerator.AveragePrice(availableStrategy, storedPriceAverage));

                                    if (availableBase > 0.0m && availableBase > symbol.MinNotionalFilter.MinNotional)
                                    {
                                        // STOP LOSS
                                        var stopLossOrder = _marketService.IsStopLossReached(availableStrategy.StopLossPercentageDown, storedPriceAverage, price);

                                        Logger.Info(LogGenerator.StopLossOrder(stopLossOrder));

                                        if (stopLossOrder.IsReadyForMarket)
                                        {
                                            Logger.Info(LogGenerator.StopLossOrderReady(price, stopLossOrder, availableStrategy));

                                            if (strategy.IsNotInTestMode)
                                            {
                                                var quantity = BinanceHelpers.ClampQuantity(symbol.LotSizeFilter.MinQuantity, symbol.LotSizeFilter.MaxQuantity, symbol.LotSizeFilter.StepSize, availableBase);
                                                var stopLossPrice = BinanceHelpers.ClampPrice(symbol.PriceFilter.MinPrice, symbol.PriceFilter.MaxPrice, price);

                                                var minNotional = quantity * stopLossPrice;

                                                if (minNotional > symbol.MinNotionalFilter.MinNotional)
                                                {
                                                    var stopLossOrderResult = await client.PlaceOrderAsync(
                                                        availableStrategy.Symbol,
                                                        OrderSide.Sell,
                                                        OrderType.StopLossLimit,
                                                        quantity: quantity,
                                                        stopPrice: BinanceHelpers.FloorPrice(symbol.PriceFilter.TickSize, stopLossPrice),
                                                        price: BinanceHelpers.FloorPrice(symbol.PriceFilter.TickSize, stopLossPrice),
                                                        timeInForce: TimeInForce.GoodTillCancel);

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
                                            else
                                                Logger.Info(LogGenerator.StopLossTest);
                                        }

                                        // SELL BASE
                                        var sellOrder = _marketService.IsGoodToSell(availableStrategy.SellPercentageUp, storedPriceAverage, price);

                                        Logger.Info(LogGenerator.SellOrder(sellOrder));

                                        if (sellOrder.IsReadyForMarket)
                                        {
                                            Logger.Info(LogGenerator.SellOrderReady(price, sellOrder, availableStrategy));

                                            if (strategy.IsNotInTestMode)
                                            {
                                                var sellOrderResult = await client.PlaceOrderAsync(
                                                    availableStrategy.Symbol,
                                                    OrderSide.Sell,
                                                    OrderType.Market,
                                                    quantity: availableBase);

                                                if (sellOrderResult.Success)
                                                {
                                                    Logger.Info(LogGenerator.SellResultStart(sellOrderResult.Data.OrderId));

                                                    if (sellOrderResult.Data.Fills.AnyAndNotNull())
                                                    {
                                                        foreach (var item in sellOrderResult.Data.Fills)
                                                        {
                                                            Logger.Info(LogGenerator.SellResult(item));
                                                        }
                                                    }

                                                    Logger.Info(LogGenerator.SellResultEnd(sellOrderResult.Data.OrderId));
                                                }
                                                else
                                                    Logger.Warn(sellOrderResult.Error.Message);
                                            }
                                            else
                                                Logger.Info(LogGenerator.SellTest);
                                        }
                                    }

                                    if (availableQuote > 0.0m && availableQuote > symbol.MinNotionalFilter.MinNotional)
                                    {
                                        // BUY - SPEND QUOTE
                                        var buyOrder = _marketService.IsGoodToBuy(availableStrategy.BuyPercentageDown, storedPriceAverage, price);

                                        Logger.Info(LogGenerator.BuyOrder(buyOrder));

                                        if (buyOrder.IsReadyForMarket)
                                        {
                                            Logger.Info(LogGenerator.BuyOrderReady(price, buyOrder, availableStrategy));

                                            if (strategy.IsNotInTestMode)
                                            {
                                                var buyOrderResult = await client.PlaceOrderAsync(
                                                    availableStrategy.Symbol,
                                                    OrderSide.Buy,
                                                    OrderType.Market,
                                                    quoteOrderQuantity: availableQuote);

                                                if (buyOrderResult.Success)
                                                {
                                                    Logger.Info(LogGenerator.BuyResultStart(buyOrderResult.Data.OrderId));

                                                    if (buyOrderResult.Data.Fills.AnyAndNotNull())
                                                    {
                                                        foreach (var item in buyOrderResult.Data.Fills)
                                                        {
                                                            Logger.Info(LogGenerator.BuyResult(item));
                                                        }
                                                    }

                                                    Logger.Info(LogGenerator.BuyResultEnd(buyOrderResult.Data.OrderId));
                                                }
                                                else
                                                    Logger.Warn(buyOrderResult.Error.Message);
                                            }
                                            else
                                                Logger.Info(LogGenerator.BuyTest);
                                        }
                                    }
                                    else
                                        Logger.Warn(LogGenerator.WarnFilterMinNotional(quoteAsset, availableQuote, symbol.MinNotionalFilter.MinNotional));
                                }
                                else
                                    Logger.Warn(currentPrice.Message);
                            }
                            else
                                Logger.Warn(LogGenerator.WarnSymbol(availableStrategy.Symbol));
                        }
                    }
                    else
                        Logger.Warn(LogGenerator.WarnKeys);
                }
            }
            else
                Logger.Warn(LogGenerator.WarnStrategy);
        }
    }
}