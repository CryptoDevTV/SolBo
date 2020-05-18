using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class FundStepValidationRule : IRule
    {
        public string RuleName => "FUND PERCENTAGE VALIDATION";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} success"
                    : $"{RuleName} error"
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.FundPercentage > 0
            && solbot.Strategy.AvailableStrategy.FundPercentage < 100;
    }
}