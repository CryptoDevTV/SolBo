using SolBo.Shared.Domain.Configs;
using System.IO;

namespace SolBo.Shared.Rules.Validation
{
    public class StoragePathValidationRule : IValidatedRule
    {
        public string RuleAttribute => "StoragePath";
        public IRuleResult RuleExecuted(Solbot solbot)
            => ValidatedRuleResult.New(
                RulePassed(solbot),
                RuleAttribute,
                $"{solbot.Strategy.AvailableStrategy.SellPercentageUp}");
        public bool RulePassed(Solbot solbot)
            => !string.IsNullOrWhiteSpace(solbot.Strategy.AvailableStrategy.StoragePath)
                && Directory.Exists(solbot.Strategy.AvailableStrategy.StoragePath);
    }
}