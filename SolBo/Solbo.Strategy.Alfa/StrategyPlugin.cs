using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Solbo.Strategy.Alfa.Job;
using SolBo.Shared.Strategies;
using System;

namespace Solbo.Strategy.Alfa
{
    internal class StrategyPlugin : StrategyPluginBase, IStrategy
    {
        public string Name() => "Alfa";
        public Tuple<IJobDetail, TriggerBuilder> StrategyRuntime()
            => CreateStrategy<StrategyJob>(Name());
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<StrategyJob>();
        }
    }
}