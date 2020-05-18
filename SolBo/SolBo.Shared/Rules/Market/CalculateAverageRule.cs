using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Messages.Rules;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Market
{
    public class CalculateAverageRule : IRule
    {
        private readonly IStorageService _storageService;
        public CalculateAverageRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public string RuleName => "CALCULATE AVERAGE";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} success"
                    : $"{RuleName} error"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            try
            {
                var storedPriceAverage = AverageContext.Average(
                    _storageService.GetValues(),
                    4,
                    solbot.Strategy.AvailableStrategy.Average);

                solbot.Communication.Average = new PriceMessage
                {
                    Current = storedPriceAverage
                };

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}