using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System;
using System.IO;

namespace SolBo.Shared.Rules.Sequence
{
    public class SetStorageSequenceRule : ISequencedRule
    {
        private readonly IStorageService _storageService;
        public SetStorageSequenceRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            try
            {
                var storagePath = Path.Combine(
                    solbot.Strategy.AvailableStrategy.StoragePath,
                    $"{solbot.Strategy.AvailableStrategy.Symbol}.txt");

                _storageService.SetPath(storagePath);
            }
            catch (Exception e)
            {
                result.Message = e.GetFullMessage();
            }
            return result;
        }
    }
}