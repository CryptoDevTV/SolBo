using Solbo.Strategy.Beta.Models;
using SolBo.Shared.Strategies.Predefined.Results;

namespace Solbo.Strategy.Beta.Rules
{
    internal interface IBetaRule
    {
        IRuleResult Result(StrategyModel strategyModel);
    }
}