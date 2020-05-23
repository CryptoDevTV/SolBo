namespace SolBo.Shared.Rules
{
    public interface IRuleResult
    {
        string Message { get; }
        bool Success { get; }
    }
}