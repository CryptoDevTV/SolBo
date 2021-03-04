using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Linq;

namespace Solbo.Strategy.Alfa.Trading
{
    public class AveragePriceRule : IAlfaRule
    {
        private readonly IFileService _fileService;
        public AveragePriceRule(IFileService fileService)
        {
            _fileService = fileService;
        }
        public IRuleResult Result(StrategyModel strategyModel)
        {
            var errors = string.Empty;
            try
            {
                var toTake = strategyModel.Average + 1;
                var lastN = _fileService.GetValues(
                    GlobalConfig.PriceFile("Alfa", strategyModel.Symbol),
                    toTake);
                var values = lastN;
                if (strategyModel.AverageType == AverageType.WITH_CURRENT)
                {
                    values = lastN.Skip(1);
                }
                else
                {
                    values = lastN.SkipLast(1);
                }
                if (!(strategyModel.Communication.BinanceSymbol is null))
                {
                    strategyModel.Communication.CurrentAverage = decimal.Round(values.Average(), strategyModel.Communication.BinanceSymbol.QuoteAssetPrecision);
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