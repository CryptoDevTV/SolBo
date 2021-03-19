﻿using Binance.Net.Interfaces;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
}