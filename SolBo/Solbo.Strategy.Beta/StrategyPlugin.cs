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
            => CreateStrategy<StrategyJob>(Name());
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<StrategyJob>();
        }
    }
}