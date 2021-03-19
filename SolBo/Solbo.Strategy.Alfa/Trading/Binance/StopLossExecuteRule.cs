using Binance.Net.Interfaces;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Extensions;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Trading.Binance
{
    public class StopLossExecuteRule : IAlfaRule
    {
        private readonly IBinanceClient _binanceClient;
        public StopLossExecuteRule(
            IBinanceClient binanceClient)
        {
            _binanceClient = binanceClient;
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