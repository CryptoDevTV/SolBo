using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Validation
{
    public class TickerValidationRule : IRule
    {
        public string RuleName => "TICKER VALIDATION";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Ticker: {solbot.Strategy.AvailableStrategy.Ticker}"
                    : $"{RuleName} error. Set Ticker property correctly."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Strategy.AvailableStrategy.Ticker == 0
            || solbot.Strategy.AvailableStrategy.Ticker == 1;
    }
}