using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Solbo.Strategy.Beta.Job;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Strategies;
using System;
using System.Collections.Generic;

namespace Solbo.Strategy.Beta
{
    internal class StrategyPlugin : StrategyPluginBase, IStrategyPlugin
    {
        public string Name() => "Beta";
        public IEnumerable<Tuple<IJobDetail, TriggerBuilder, string>> StrategyRuntime(IEnumerable<Pair> pairs)
        {
            var result = new List<Tuple<IJobDetail, TriggerBuilder, string>>();

            foreach (var pair in pairs)
            {
                result.Add(CreateStrategy<StrategyJob>(Name(), pair.Symbol));
            }
            return result;
        }
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<StrategyJob>();
        }
    }
}