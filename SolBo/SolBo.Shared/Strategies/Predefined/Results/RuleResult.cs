namespace SolBo.Shared.Strategies.Predefined.Results
{
    public class RuleResult : IRuleResult
    {
        public RuleResult() { }
        public RuleResult(string message)
        {
            Message = message;
        }
        public string Message { get; private set; }
        public bool Success => string.IsNullOrWhiteSpace(Message);
    }
}