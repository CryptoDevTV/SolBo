using Quartz;
using System;
using System.IO;

namespace SolBo.Shared.Strategies
{
    public abstract class StrategyPluginBase
    {
        protected string ConfigPath(string pluginName)
            => Path.Combine(AppContext.BaseDirectory, "strategies", pluginName, "strategy.json");

        protected Tuple<IJobDetail, TriggerBuilder> CreateStrategy<T>(string name) where T : IJob
        {
            var jobDetail = JobBuilder.Create<T>()
                    .WithIdentity($"{name}Job")
                    .Build();

            jobDetail.JobDataMap["path"] = ConfigPath(name);

            var jobBuilder = TriggerBuilder.Create()
                .WithIdentity($"{name}Trigger")
                .StartNow();

            return new Tuple<IJobDetail, TriggerBuilder>(jobDetail, jobBuilder);
        }
    }
}