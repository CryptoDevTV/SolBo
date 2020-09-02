using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class CalculateAverageSequenceRule : ISequencedRule
    {
        public string SequenceName => "AVERAGE";
        private readonly IStorageService _storageService;
        private readonly IMarketService _marketService;
        public CalculateAverageSequenceRule(
            IStorageService storageService,
            IMarketService marketService)
        {
            _storageService = storageService;
            _marketService = marketService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                var storedPriceAverage = _marketService.Average(
                    solbot.Strategy.AvailableStrategy.AverageType,
                    _storageService.GetValues(),
                    solbot.Communication.Symbol.QuoteAssetPrecision,
                    solbot.Strategy.AvailableStrategy.Average);

                solbot.Communication.Average = new PriceMessage
                {
                    Current = storedPriceAverage
                };

                result.Success = true;
                result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{storedPriceAverage}");
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = LogGenerator.SequenceException(SequenceName, e);
            }
            return result;
        }
    }
}