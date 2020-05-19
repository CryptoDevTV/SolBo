using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class FundStepValidationRule : IRule
    {
        public string RuleName => "FUND PERCENTAGE VALIDATION";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => FundPercentage: {solbot.Strategy.AvailableStrategy.FundPercentage}."
                    : $"{RuleName} error. Set FundPercentage."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.FundPercentage > 0
            && solbot.Strategy.AvailableStrategy.FundPercentage < 100;
    }
}