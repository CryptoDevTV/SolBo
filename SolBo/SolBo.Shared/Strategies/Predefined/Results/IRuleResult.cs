namespace SolBo.Shared.Strategies.Predefined.Results
{
    public interface IRuleResult
    {
        string Message { get; }
        bool Success { get; }
    }
}