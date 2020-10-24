using FluentValidation.Results;

namespace SolBo.Shared.Strategies.Predefined.Results
{
    public class RuleResult : IRuleResult
    {
        public RuleResult() { }
        public RuleResult(ValidationResult validationResult)
        {
            Success = validationResult.IsValid;
            Message = validationResult.ToString();
        }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}