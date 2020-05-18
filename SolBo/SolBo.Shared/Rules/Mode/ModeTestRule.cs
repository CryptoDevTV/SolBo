using NLog;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Rules.Mode.Test;
using SolBo.Shared.Services;
using System.Collections.Generic;

namespace SolBo.Shared.Rules.Mode
{
    public class ModeTestRule : IRule
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");
        private readonly IMarketService _marketService;
        private readonly ICollection<IRule> _rules = new HashSet<IRule>();
        public ModeTestRule(IMarketService marketService)
        {
            _marketService = marketService;
        }

        public string RuleName => "TEST MODE";

        public ResultRule ExecutedRule(Solbot solbot)
        {
            _rules.Add(new StopLossStepRule(_marketService));
            _rules.Add(new StopLossExecuteTestRule());

            _rules.Add(new SellStepRule(_marketService));
            _rules.Add(new SellExecuteTestRule());

            _rules.Add(new BuyStepRule(_marketService));
            _rules.Add(new BuyExecuteTestRule());

            Logger.Info($"{RuleName} START");

            foreach (var item in _rules)
            {
                var result = item.ExecutedRule(solbot);

                Logger.Info($"{result.Message}");
            }

            Logger.Info($"{RuleName} END");

            return new ResultRule
            {
                Success = RulePassed(solbot),
                Message = $"{RuleName} EXECUTED"
            };
        }

        public bool RulePassed(Solbot solbot)
            => true;
    }
}