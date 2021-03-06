using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Trading
{
    public class CreateStorageRule : IAlfaRule
    {
        private readonly IFileService _fileService;
        private readonly string _strategy;
        public CreateStorageRule(
            IFileService fileService,
            string strategy)
        {
            _fileService = fileService;
            _strategy = strategy;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var storageFile = GlobalConfig.StorageFile(_strategy, strategyModel.Symbol);
                if (!_fileService.Exist(storageFile))
                {
                    var newStorage = new StorageRootModel
                    {
                        Action = new StorageActionModel
                        {
                            BoughtPrice = 0m,
                            StopLossCurrentCycle = 0,
                            StopLossReached = false
                        }
                    };
                    SyncExt.RunSync(() => _fileService.SerializeAsync(storageFile, newStorage));
                }
            }
            catch (Exception ex)
            {
                errors += ex.GetFullMessage();
            }
            return new RuleResult(errors);
        }
    }
}