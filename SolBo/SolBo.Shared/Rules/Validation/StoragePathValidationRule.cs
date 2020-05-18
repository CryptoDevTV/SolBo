using SolBo.Shared.Domain.Configs;
using System.IO;

namespace SolBo.Shared.Rules.Validation
{
    public class StoragePathValidationRule : IRule
    {
        public string RuleName => "STORAGE PATH VALIDATION";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Path: {solbot.Strategy.AvailableStrategy.StoragePath}."
                    : $"{RuleName} error. Set StoragePath property correctly."
            };
        }

        public bool RulePassed(Solbot solbot)
            => !string.IsNullOrWhiteSpace(solbot.Strategy.AvailableStrategy.StoragePath)
                && Directory.Exists(solbot.Strategy.AvailableStrategy.StoragePath);
    }
}