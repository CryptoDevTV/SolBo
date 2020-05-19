using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Strategy
{
    public class StrategyRule : IRule
    {
        public string RuleName => "ACTIVE STRATEGY";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SELECTED => ActiveId: {solbot.Strategy.ActiveId}."
                    : $"{RuleName} not selected. Set ActiveId."
            };
        }
        public bool RulePassed(Solbot solbot)
            => !(solbot.Strategy.AvailableStrategy is null);
    }
}