using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Solbo.Strategy.Alfa.Job;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Strategies;
using System;

namespace Solbo.Strategy.Alfa
{
    internal class StrategyPlugin : StrategyPluginBase, IStrategyPlugin
    {
        public string Name() => $"Alfa";
        public Tuple<IJobDetail, TriggerBuilder, string> StrategyRuntime(Pair pair)
            => CreateStrategy<StrategyJob>(Name(), pair.Symbol);
        public void Configure(IServiceCollection services)
            => services.AddSingleton<StrategyJob>();
    }
}