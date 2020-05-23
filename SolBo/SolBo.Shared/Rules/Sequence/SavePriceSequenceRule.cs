using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class SavePriceSequenceRule : ISequencedRule
    {
        private readonly IStorageService _storageService;
        public SavePriceSequenceRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                _storageService.SaveValue(solbot.Communication.Price.Current);
            }
            catch (Exception e)
            {
                result.Message = e.GetFullMessage();
            }
            return result;
        }
    }
}