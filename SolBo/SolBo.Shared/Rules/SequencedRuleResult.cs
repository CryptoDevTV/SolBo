namespace SolBo.Shared.Rules
{
    public class SequencedRuleResult : IRuleResult
    {
        public string Message { get; set; }
        public bool Success
            => string.IsNullOrWhiteSpace(Message);
    }
}