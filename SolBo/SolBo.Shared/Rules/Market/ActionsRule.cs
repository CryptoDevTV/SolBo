using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Market
{
    public class ActionsRule : IRule
    {
        public string RuleName => "BOUGHT VALIDATION";
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
            => solbot.Actions.Bought == 0
            || solbot.Actions.Bought == 1;
    }
}