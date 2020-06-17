using System;
using System.Collections.Generic;
using System.Text;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class BuyExecuteMarketRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.BUYING;
        private readonly IMarketService _marketService;
        public BuyExecuteMarketRule(IMarketService marketService)
        {
            _marketService = marketService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            throw new NotImplementedException();
        }
    }
}