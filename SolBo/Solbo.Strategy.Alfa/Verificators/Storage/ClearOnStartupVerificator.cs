using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Verificators.Storage
{
    internal class ClearOnStartupVerificator : IAlfaRule
    {
        private readonly IFileService _fileService;
        private readonly bool _runForTheFirstTime = false;
        private readonly string _strategy;
        public ClearOnStartupVerificator(
            IFileService fileService, 
            DateTimeOffset? runLastTime,
            string strategy)
        {
            _fileService = fileService;
            _runForTheFirstTime = runLastTime is null;
            _strategy = strategy;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            if (strategyModel.ClearOnStartup && _runForTheFirstTime)
            {
                var priceFile = GlobalConfig.PriceFile(_strategy, strategyModel.Symbol);
                var backupFile = GlobalConfig.PriceFileBackup(_strategy, strategyModel.Symbol);
                if (_fileService.Exist(priceFile))
                {
                    _fileService.CreateBackup(priceFile, backupFile);
                    _fileService.ClearFile(priceFile);
                }
            }
            return new RuleResult(errors);
        }
    }
}