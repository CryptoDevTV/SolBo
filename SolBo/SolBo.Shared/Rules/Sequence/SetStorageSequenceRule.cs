using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Sequence
{
    public class SetStorageSequenceRule : ISequencedRule
    {
        public string SequenceName => "STORAGE";
        private readonly IStorageService _storageService;
        public SetStorageSequenceRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var storagePath = GlobalConfig.PriceFile("",solbot.Strategy.AvailableStrategy.Symbol);

            var result = new SequencedRuleResult();
            try
            {
                _storageService.SetPath(storagePath);

                result.Success = true;
                result.Message = LogGenerator.SequenceSuccess(SequenceName, storagePath);
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