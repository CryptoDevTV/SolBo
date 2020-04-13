using Binance.Net;
using NLog;
using Quartz;
using SolBo.Shared.Domain.Configs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolBo.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class BuyDeepSellHighJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

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
                    Logger.Info($"Average price from last {priceAvg.Data.Minutes}min for {activeStrategy.Symbol} on {exchange.Name} is {priceAvg.Data.Price}");
                }
                else
                {
                    Logger.Warn(priceAvg.Error.Message);
                }
            }
        }
    }
}