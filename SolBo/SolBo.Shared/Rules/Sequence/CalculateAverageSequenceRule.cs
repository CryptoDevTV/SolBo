using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class CalculateAverageSequenceRule : ISequencedRule
    {
        private readonly IStorageService _storageService;
        public CalculateAverageSequenceRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                var count = _storageService.GetValues().Count;

                var storedPriceAverage = AverageContext.Average(
                    _storageService.GetValues(),
                    4,
                    solbot.Strategy.AvailableStrategy.Average);

                solbot.Communication.Average = new PriceMessage
                {
                    Current = storedPriceAverage,
                    Count = count < solbot.Strategy.AvailableStrategy.Average
                    ? count
                    : solbot.Strategy.AvailableStrategy.Average
                };
            }
            catch (Exception e)
            {
                result.Message = e.GetFullMessage();
            }
            return result;
        }
    }
}