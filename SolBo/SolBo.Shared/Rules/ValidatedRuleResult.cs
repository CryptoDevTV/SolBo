namespace SolBo.Shared.Rules
{
    public class ValidatedRuleResult : IRuleResult
    {
        public static ValidatedRuleResult New(bool result, string ruleAttribute, string attributeValue)
            => new ValidatedRuleResult
            {
                Success = result,
                Message = result
                    ? $"Validation SUCCESS => {ruleAttribute}" 
                    : $"Validation ERROR => {ruleAttribute} => Value => {attributeValue} => BAD"
            };
        protected ValidatedRuleResult() { }
        public string Message { get; private set; }
        public bool Success { get; private set; }
    }
}