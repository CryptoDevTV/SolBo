using SolBo.Shared.Domain.Statics;

namespace SolBo.Shared.Rules
{
    public class ValidatedRuleResult : IRuleResult
    {
        public static ValidatedRuleResult New(bool result, string ruleAttribute, string attributeValue)
            => new ValidatedRuleResult
            {
                Success = result,
                Message = result
                    ? LogGenerator.ValidationSuccess(ruleAttribute)
                    : LogGenerator.ValidationError(ruleAttribute, attributeValue)
            };
        protected ValidatedRuleResult() { }
        public string Message { get; private set; }
        public bool Success { get; private set; }
    }
}