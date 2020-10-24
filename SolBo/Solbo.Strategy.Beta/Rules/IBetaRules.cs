using Solbo.Strategy.Beta.Models;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Rules
{
    internal interface IBetaRules
    {
        IRuleResult Result(StrategyRootModel strategyRootModel);
    }
}