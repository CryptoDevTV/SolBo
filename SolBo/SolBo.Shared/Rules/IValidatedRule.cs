using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules
{
    public interface IValidatedRule : IRule
    {
        bool RulePassed(Solbot solbot);
        string RuleAttribute { get; }
    }
}