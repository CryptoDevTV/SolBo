using SolBo.Shared.Contexts;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
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
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Average: {solbot.Communication.Average.Current}, From last: {solbot.Communication.Average.Count}"
                    : $"{RuleName} error. {Message}"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            try
            {
                var count = _storageService.GetValues().Count;

                var storedPriceAverage = AverageContext.Average(
                    _storageService.GetValues(),
                    4,
                    solbot.Strategy.AvailableStrategy.Average);

                solbot.Communication.Average = new PriceMessage
                {
                    Current = storedPriceAverage,
                    Count = count < solbot.Strategy.AvailableStrategy.Average
                    ? count
                    : solbot.Strategy.AvailableStrategy.Average
                };

                return true;
            }
            catch (Exception e)
            {
                Message = e.GetFullMessage();

                return false;
            }
        }
    }
}