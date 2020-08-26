using SolBo.Shared.Domain.Configs;
using System.Linq;

namespace SolBo.Shared.Rules.Validation
{
    public class ApiCredentialsValidationRule : IValidatedRule
    {
        public string RuleAttribute => "ApiCredentials";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"ApiKey => OK => ApiSecret => OK");
        public bool RulePassed(Solbot solbot)
            => !solbot.Exchange.ApiKey.Any(char.IsWhiteSpace) && !solbot.Exchange.ApiSecret.Any(char.IsWhiteSpace);
    }
}