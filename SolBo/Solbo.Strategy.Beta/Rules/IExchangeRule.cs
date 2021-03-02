using Solbo.Strategy.Beta.Models;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Rules
{
    internal interface IExchangeRule
    {
        IRuleResult Result(StrategyRootExchange strategyRootExchange);
    }
}