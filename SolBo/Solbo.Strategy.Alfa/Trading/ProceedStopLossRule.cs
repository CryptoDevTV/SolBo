using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Trading
{
    public class ProceedStopLossRule : IAlfaRule
    {
        private readonly IFileService _fileService;
        private readonly string _strategy;
        public ProceedStopLossRule(
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
                var model = SyncExt.RunSync(() => _fileService.DeserializeAsync<StorageRootModel>(storageFile));

                if (model.Action.StopLossReached
                    && model.Action.BoughtPrice == 0
                    && model.Action.StopLossCurrentCycle < strategyModel.StopLossPauseCycles)
                {
                    model.Action.StopLossCurrentCycle++;
                    errors = "Paused after SL reached.";

                    SyncExt.RunSync(() => _fileService.SerializeAsync(storageFile, model));
                }
                else if (!model.Action.StopLossReached
                    && model.Action.BoughtPrice > 0)
                {
                    // waiting for sell opportunity
                }
                else
                {
                    model.Action.StopLossCurrentCycle = 0;
                    model.Action.BoughtPrice = 0;
                    model.Action.StopLossReached = false;

                    SyncExt.RunSync(() => _fileService.SerializeAsync(storageFile, model));
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