using System;
using System.IO;

namespace SolBo.Shared.Strategies
{
    public abstract class StrategyPluginBase
    {
        protected string ConfigPath(string pluginName)
            => Path.Combine(AppContext.BaseDirectory, "strategies", pluginName, "strategy.json");
    }
}