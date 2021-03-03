using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;

namespace Solbo.Strategy.Alfa.Trading
{
    public class SavePriceRule : IAlfaRule
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
                    GlobalConfig.PriceFile("Alfa", strategyModel.Symbol),
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