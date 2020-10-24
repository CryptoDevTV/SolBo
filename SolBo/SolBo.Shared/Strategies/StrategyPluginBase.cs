using Quartz;
using System;
using System.IO;

namespace SolBo.Shared.Strategies
{
    public abstract class StrategyPluginBase
    {
        protected string ConfigPath(string pluginName)
            => Path.Combine(AppContext.BaseDirectory, "strategies", pluginName, "strategy.json");

        protected Tuple<IJobDetail, TriggerBuilder, string> CreateStrategy<T>(string name, string symbol) where T : IJob
        {
            var jobDetail = JobBuilder.Create<T>()
                    .WithIdentity($"{name}_{symbol}_Job")
                    .Build();

            jobDetail.JobDataMap["path"] = ConfigPath(name);
            jobDetail.JobDataMap["name"] = name;
            jobDetail.JobDataMap["symbol"] = symbol;

            var jobBuilder = TriggerBuilder.Create()
                .WithIdentity($"{name}_{symbol}_Trigger")
                .StartNow();

            return new Tuple<IJobDetail, TriggerBuilder, string>(jobDetail, jobBuilder, symbol);
        }
    }
}