using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules
{
    public interface IRule
    {
        bool RulePassed(Solbot solbot);
        ResultRule ExecutedRule(Solbot solbot);
        string RuleName { get; }
    }
}