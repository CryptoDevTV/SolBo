using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using System;

namespace SolBo.Shared.Rules.Market
{
    public class SavePriceRule : IRule
    {
        private readonly IStorageService _storageService;
        public SavePriceRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public string RuleName => "SAVE PRICE";
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
                _storageService.SaveValue(solbot.Communication.Price.Current);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}