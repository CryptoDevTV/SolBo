using Binance.Net;
using NLog;
using Quartz;
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
        private readonly ICalculationService _calculationService;

        public BuyDeepSellHighJob(
            IStorageService storageService,
            ICalculationService calculationService)
        {
            _storageService = storageService;
            _calculationService = calculationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var exchange = (context.JobDetail.JobDataMap["Exchanges"] as IList<Exchange>).FirstOrDefault();

            var strategy = context.JobDetail.JobDataMap["Strategy"] as Strategy;

            var activeStrategy = strategy.Available.FirstOrDefault(s => s.Id == strategy.ActiveId);

            using (var client = new BinanceClient())
            {
                var priceAvg = await client.GetCurrentAvgPriceAsync(activeStrategy.Symbol);

                if (priceAvg.Success)
                {
                    var price = decimal.Round(priceAvg.Data.Price, 2);

                    Logger.Info($"Average price from last {priceAvg.Data.Minutes}min for {activeStrategy.Symbol} on {exchange.Name} is {price}");

                    _storageService.SaveValue(price);

                    var storedPriceAverage = _calculationService.CalculateAverage(_storageService.GetValues());

                    Logger.Info($"Average price is {storedPriceAverage}");

                    var canIbuy = _calculationService.IsGoodToBuy(activeStrategy.BidRatio, storedPriceAverage, price);

                    if(canIbuy)
                    {

                    }
                }
                else
                {
                    Logger.Warn(priceAvg.Error.Message);
                }
            }
        }
    }
}