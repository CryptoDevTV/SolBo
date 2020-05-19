using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossStepValidationRule : IRule
    {
        public string RuleName => "STOP LOSS PERCENTAGE VALIDATION";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => StopLossPercentageDown: {solbot.Strategy.AvailableStrategy.StopLossPercentageDown}."
                    : $"{RuleName} error. Set StopLossPercentageDown."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.StopLossPercentageDown > 0;
    }
}