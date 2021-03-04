using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies.Predefined.Results;
using System;
using System.Linq;

namespace Solbo.Strategy.Beta.Trading
{
    public class AveragePriceRule : IBetaRules
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
                    GlobalConfig.PriceFile("Beta", strategyModel.Symbol),
                    strategyModel.Average);
                var values = lastN;
                if (strategyModel.AverageType == AverageType.WITH_CURRENT)
                {
                    values = lastN.Skip(1);
                }
                else
                {
                    values = lastN.SkipLast(1);
                }
                if (!(strategyModel.Communication.KucoinSymbol is null))
                {
                    strategyModel.Communication.CurrentAverage = decimal.Round(
                        values.Average(),
                        BitConverter.GetBytes(decimal.GetBits(strategyModel.Communication.KucoinSymbol.QuoteIncrement)[3])[2]);
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