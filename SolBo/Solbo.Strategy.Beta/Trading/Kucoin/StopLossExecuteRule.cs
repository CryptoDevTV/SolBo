using Kucoin.Net.Interfaces;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Beta.Trading.Kucoin
{
    public class StopLossExecuteRule : IBetaRule
    {
        private readonly IKucoinClient _kucoinClient;
        public StopLossExecuteRule(
            IKucoinClient kucoinClient)
        {
            _kucoinClient = kucoinClient;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                if (strategyModel.Communication.IsPossibleStopLoss)
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