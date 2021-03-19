using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Beta.Trading.Kucoin
{
    public class SellStepRule : IBetaRule
    {
        private readonly IFileService _fileService;
        private readonly string _strategy;
        public SellStepRule(
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

                if (model.Action.BoughtBefore)
                {
                    strategyModel.Communication.IsPossibleSell = true;
                }
                else
                {
                    strategyModel.Communication.IsPossibleSell = false;
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