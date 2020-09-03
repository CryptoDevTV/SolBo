using SolBo.Shared.Domain.Configs;
using System.Linq;

namespace SolBo.Shared.Rules.Validation
{
    public class ApiCredentialsValidationRule : IValidatedRule
    {
        public string RuleAttribute => "ApiCredentials";
        private readonly Exchange _exchange;
        public ApiCredentialsValidationRule(Exchange exchange)
        {
            _exchange = exchange;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"ApiKey => OK => ApiSecret => OK");
        public bool RulePassed(Solbot solbot)
            => !_exchange.ApiKey.Any(char.IsWhiteSpace) && !_exchange.ApiSecret.Any(char.IsWhiteSpace);
    }
}