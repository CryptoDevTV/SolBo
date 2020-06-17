using System;
using System.Collections.Generic;
using System.Text;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Mode.Production
{
    public class SellExecuteMarketRule : IMarketRule
    {
        public MarketOrderType MarketOrder => MarketOrderType.SELLING;
        private readonly IMarketService _marketService;
        public SellExecuteMarketRule(IMarketService marketService)
        {
            _marketService = marketService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            throw new NotImplementedException();
        }
    }
}