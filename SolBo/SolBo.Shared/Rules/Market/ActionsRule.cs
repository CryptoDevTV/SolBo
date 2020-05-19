using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Market
{
    public class ActionsRule : IRule
    {
        public string RuleName => "BOUGHT VALIDATION";
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Bought: {solbot.Actions.Bought}."
                    : $"{RuleName} error. Set Bought."
            };
        }
        public bool RulePassed(Solbot solbot)
            => solbot.Actions.Bought == 0
            || solbot.Actions.Bought == 1;
    }
}