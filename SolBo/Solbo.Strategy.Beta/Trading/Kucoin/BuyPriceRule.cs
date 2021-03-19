using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Beta.Trading.Kucoin
{
    public class BuyPriceRule : IBetaRule
    {
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                if (strategyModel.Communication.IsPossibleBuy)
                {

                }
            }
            catch (Exception ex)
            {
                errors += ex.GetFullMessage();
            }
            return new RuleResult(errors);
        }
    }
}