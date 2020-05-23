using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules
{
    public interface IRule
    {
        IRuleResult RuleExecuted(Solbot solbot);
    }
}