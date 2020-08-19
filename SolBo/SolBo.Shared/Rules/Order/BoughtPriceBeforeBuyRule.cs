using SolBo.Shared.Domain.Configs;

namespace SolBo.Shared.Rules.Order
{
    public class BoughtPriceBeforeBuyRule : IOrderRule
    {
        public string OrderStep => "BoughtPriceBeforeBuy";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Actions.BoughtPrice == 0;
            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? "Solbo can set a transaction, to previous opened by Solb "
                    : "Solbo will wait to close previous transaction"
            };
        }
    }
}