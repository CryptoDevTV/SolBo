using Solbo.Strategy.Alfa.Models;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Alfa.Rules
{
    internal interface IAlfaRule
    {
        IRuleResult Result(StrategyRootModel strategyRootModel);
    }
}