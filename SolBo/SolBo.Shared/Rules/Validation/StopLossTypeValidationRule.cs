using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class StopLossTypeValidationRule : IRule
    {
        public string RuleName => "STOP LOSS TYPE VALIDATION";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => StopLossType: {solbot.Strategy.AvailableStrategy.StopLossType}."
                    : $"{RuleName} error. Set StopLossType property correctly."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.StopLossType == 0
            || solbot.Strategy.AvailableStrategy.StopLossType == 1;
    }
}