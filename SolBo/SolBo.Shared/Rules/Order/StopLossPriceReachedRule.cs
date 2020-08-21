using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class StopLossPriceReachedRule : IOrderRule
    {
        public string OrderStep => "StopLossPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.StopLoss.PriceReached;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Stop loss price reached ({solbot.Communication.StopLoss.PriceReached})"
                    : $"Stop loss price not reached ({solbot.Communication.StopLoss.PriceReached})"
            };
        }
    }
}