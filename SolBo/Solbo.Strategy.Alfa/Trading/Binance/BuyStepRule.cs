using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Trading.Binance
{
    public class BuyStepRule : IAlfaRule
    {
        private readonly IFileService _fileService;
        private readonly string _strategy;
        public BuyStepRule(
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

                if (model.Action.SellBefore)
                {
                    strategyModel.Communication.IsPossibleBuy = true;
                }
                else
                {
                    strategyModel.Communication.IsPossibleBuy = false;
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