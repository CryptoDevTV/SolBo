using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SolBo.Shared.Domain.Configs;
using System;
using System.Collections.Generic;

namespace SolBo.Shared.Strategies
{
    public interface IStrategyPlugin
    {
        string Name();
        IEnumerable<Tuple<IJobDetail, TriggerBuilder, string>> StrategyRuntime(IEnumerable<Pair> pairs);
        void Configure(IServiceCollection services);
    }
}