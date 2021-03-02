using Solbo.Strategy.Alfa.Models;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Rules
{
    internal interface IExchangeRule
    {
        IRuleResult Result(StrategyRootExchange strategyRootExchange);
    }
}