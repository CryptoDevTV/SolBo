using Binance.Net.Interfaces;
using Kucoin.Net.Interfaces;
using NLog;
using Quartz;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using SolBo.Shared.Rules;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Agent.Strategies
{
    [DisallowConcurrentExecution]
    public class RollingPriceJob : IJob
    {
        public StrategyType StrategiesType => StrategyType.ROLLING_PRICE;
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        private readonly IBinanceClient _binanceClient;
        private readonly IKucoinClient _kucoinClient;

        private readonly ICollection<IRule> _rules = new HashSet<IRule>();

        public RollingPriceJob(
            IBinanceClient binanceClient,
            IKucoinClient kucoinClient)
        {
            _binanceClient = binanceClient;
            _kucoinClient = kucoinClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobArgs = context.JobDetail.JobDataMap["args"] as string;
            var job = JsonSerializer.Deserialize<RollingPrice>(jobArgs);

            var botArgs = context.JobDetail.JobDataMap["exchange"] as string;
            var exchange = JsonSerializer.Deserialize<Exchange>(botArgs);
            try
            {
                Logger.Info($"RP - {job.Id} - {job.Symbol} on {exchange.Type.GetDescription()}");
            }
            catch (Exception e)
            {
                Logger.Fatal($"{Environment.NewLine}[{StrategiesType.GetDescription()}] Message => {e.Message}{Environment.NewLine} StackTrace => {e.StackTrace}");
            }
        }
    }
}