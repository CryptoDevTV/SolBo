using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class SellStepValidationRule : IRule
    {
        public string RuleName => "SELL PERCENTAGE VALIDATION";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => SellPercentageUp: {solbot.Strategy.AvailableStrategy.SellPercentageUp}."
                    : $"{RuleName} error. Set SellPercentageUp property correctly."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.SellPercentageUp > 0;
    }
}