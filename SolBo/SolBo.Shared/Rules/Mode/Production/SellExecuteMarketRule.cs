using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class SellExecuteMarketRule : IMarketRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;
        private readonly IBinanceClient _binanceClient;
        public SellExecuteMarketRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
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
                    var sellOrderResult = _binanceClient.PlaceOrder(
                        solbot.Strategy.AvailableStrategy.Symbol,
                        OrderSide.Sell,
                        OrderType.Market,
                        quantity: quantity);

                    if (!(sellOrderResult is null))
                    {
                        result = sellOrderResult.Success;

                        if (sellOrderResult.Success)
                        {
                            Logger.Info(LogGenerator.TradeResultStart(sellOrderResult.Data.OrderId));

                            if (sellOrderResult.Data.Fills.AnyAndNotNull())
                            {
                                foreach (var item in sellOrderResult.Data.Fills)
                                {
                                    Logger.Info(LogGenerator.TradeResult(item));
                                }
                            }

                            Logger.Info(LogGenerator.TradeResultEnd(sellOrderResult.Data.OrderId));
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