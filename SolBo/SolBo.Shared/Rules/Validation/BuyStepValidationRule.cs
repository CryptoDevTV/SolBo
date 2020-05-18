using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class BuyStepValidationRule : IRule
    {
        public string RuleName => "BUY PERCENTAGE VALIDATION";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => BuyPercentageDown: {solbot.Strategy.AvailableStrategy.BuyPercentageDown}."
                    : $"{RuleName} error. Set BuyPercentageDown property correctly."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.BuyPercentageDown > 0;
    }
}