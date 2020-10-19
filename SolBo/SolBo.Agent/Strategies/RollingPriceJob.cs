using NLog;
using Quartz;
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
        public StrategiesType StrategiesType => StrategiesType.ROLLING_PRICE;
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public async Task Execute(IJobExecutionContext context)
        {
            var botArgs = context.JobDetail.JobDataMap["args"] as string;
            var job = JsonSerializer.Deserialize<RollingPrice>(botArgs);
            try
            {
                Logger.Info($"RP - {job.Id} - {job.Symbol}");
            }
            catch (Exception e)
            {
                Logger.Fatal($"{Environment.NewLine}[{StrategiesType.GetDescription()}] Message => {e.Message}{Environment.NewLine} StackTrace => {e.StackTrace}");
            }
        }
    }
}