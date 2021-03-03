using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Beta.Trading
{
    public class SavePriceRule : IBetaRules
    {
        private readonly IFileService _fileService;
        public SavePriceRule(IFileService fileService)
        {
            _fileService = fileService;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                _fileService.SaveValue(
                    GlobalConfig.PriceFile("Beta", strategyModel.Symbol),
                    strategyModel.Communication.CurrentPrice.GetValueOrDefault());
            }
            catch (Exception ex)
            {
                errors += ex.GetFullMessage();
            }
            return new RuleResult(errors);
        }
    }
}