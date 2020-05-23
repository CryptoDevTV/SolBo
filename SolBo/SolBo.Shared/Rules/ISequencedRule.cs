namespace SolBo.Shared.Rules
{
    public interface ISequencedRule : IRule
    {
        string SequenceName { get; }
    }
}