using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Solbo.Strategy.Beta.Job;
using SolBo.Shared.Strategies;
using System;

namespace Solbo.Strategy.Beta
{
    internal class StrategyPlugin : StrategyPluginBase, IStrategy
    {
        public string Name() => "Beta";
        public Tuple<IJobDetail, TriggerBuilder> StrategyRuntime()
        {
            var jobDetail = JobBuilder.Create<StrategyJob>()
                    .WithIdentity($"{Name()}Job")
                    .Build();

            jobDetail.JobDataMap["path"] = ConfigPath(Name());

            var jobBuilder = TriggerBuilder.Create()
                .WithIdentity($"{Name()}Trigger")
                .StartNow();

            return new Tuple<IJobDetail, TriggerBuilder>(jobDetail, jobBuilder);
        }

        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<StrategyJob>();
        }
    }
}