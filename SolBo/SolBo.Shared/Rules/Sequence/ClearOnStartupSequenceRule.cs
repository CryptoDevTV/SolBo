using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;

namespace SolBo.Shared.Rules.Sequence
{
    public class ClearOnStartupSequenceRule : ISequencedRule
    {
        public string SequenceName => "ClearOnStartup";
        private readonly IStorageService _storageService;
        private readonly bool _runForTheFirstTime = false;
        public ClearOnStartupSequenceRule(IStorageService storageService, DateTimeOffset? runLastTime)
        {
            _storageService = storageService;
            _runForTheFirstTime = runLastTime is null;
        }
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var result = new SequencedRuleResult();
            var symbol = solbot.Strategy.AvailableStrategy.Symbol;
            if (solbot.Strategy.AvailableStrategy.ClearOnStartup && _runForTheFirstTime)
            {
                try
                {
                    if (_storageService.Exist(symbol))
                    {
                        var backup = _storageService.CreateBackup(symbol);
                        _storageService.ClearFile(symbol);

                        result.Success = true;
                        result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{backup}");
                    }
                    else
                    {
                        result.Success = true;
                        result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{solbot.Strategy.AvailableStrategy.ClearOnStartup}");
                    }
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Message = LogGenerator.SequenceException(SequenceName, e);
                }
            }
            else
            {
                result.Success = true;
                result.Message = LogGenerator.SequenceSuccess(SequenceName, $"{solbot.Strategy.AvailableStrategy.ClearOnStartup}");
            }
            return result;
        }
    }
}