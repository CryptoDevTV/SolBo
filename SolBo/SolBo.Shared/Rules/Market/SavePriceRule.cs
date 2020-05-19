using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
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
        public string Message { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Price: {solbot.Communication.Price.Current}"
                    : $"{RuleName} error. {Message}"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            try
            {
                _storageService.SaveValue(solbot.Communication.Price.Current);

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