using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SolBo.Shared.Domain.Configs;
using System;

namespace SolBo.Shared.Strategies
{
    public interface IStrategyPlugin
    {
        string Name();
        Tuple<IJobDetail, TriggerBuilder, string> StrategyRuntime(Pair pair);
        void Configure(IServiceCollection services);
    }
}