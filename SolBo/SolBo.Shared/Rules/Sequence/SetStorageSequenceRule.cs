using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System;
using System.IO;

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
            var storagePath = Path.Combine(
                    solbot.Strategy.AvailableStrategy.StoragePath,
                    $"{solbot.Strategy.AvailableStrategy.Symbol}.txt");

            var result = new SequencedRuleResult();
            try
            {
                _storageService.SetPath(storagePath);

                result.Success = true;
                result.Message = $"{SequenceName} SUCCESS => {storagePath}";
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = $"{SequenceName} ERROR => {e.GetFullMessage()}";
            }
            return result;
        }
    }
}