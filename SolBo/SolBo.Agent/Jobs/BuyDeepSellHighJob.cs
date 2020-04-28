using Binance.Net;
using NLog;
using Quartz;
using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using System.Collections.Generic;
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
            var exchange = (context.JobDetail.JobDataMap["Exchanges"] as IList<Exchange>).FirstOrDefault();

            var strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;

            var availableStrategy = strategy.Available.FirstOrDefault(s => s.Id == strategy.ActiveId);

            using (var client = new BinanceClient())
            {
                var tickerContext = new TickerContext(client);

                var currentAvgPrice = await tickerContext.GetPriceValue(availableStrategy);

                if (currentAvgPrice.Success)
                {
                    var price = currentAvgPrice.Result;

                    Logger.Info($"Average price ({availableStrategy.Ticker}) for {availableStrategy.Symbol} on {exchange.Name} is {price}");

                    _storageService.SaveValue(price);

                    var storedPriceAverage = AverageContext.Average(_storageService.GetValues(), availableStrategy.Average);

                    Logger.Info($"Average price for last {availableStrategy.Average} is {storedPriceAverage}");

                    var canIbuy = _marketService.IsGoodToBuy(availableStrategy.BidRatio, storedPriceAverage, price);

                    if(canIbuy)
                    {

                    }
                }
                else
                {
                    Logger.Warn(currentAvgPrice.Message);
                }
            }
        }
    }
}