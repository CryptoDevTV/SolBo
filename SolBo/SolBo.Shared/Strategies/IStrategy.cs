using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;

namespace SolBo.Shared.Strategies
{
    public interface IStrategy
    {
        string Name();
        Tuple<IJobDetail, TriggerBuilder> StrategyRuntime();
        void Configure(IServiceCollection services);
    }
}