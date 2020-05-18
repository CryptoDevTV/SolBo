using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using System;
using System.IO;

namespace SolBo.Shared.Rules.Storage
{
    public class SetStorageRule : IRule
    {
        private readonly IStorageService _storageService;
        public SetStorageRule(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public string RuleName => "SET STORAGE";
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Storage set."
                    : $"{RuleName} error. Strage added."
            }
        }
        public bool RulePassed(Solbot solbot)
        {
            try
            {
                _storageService.SetPath(
                Path.Combine(
                    solbot.Strategy.AvailableStrategy.StoragePath, $"{solbot.Strategy.AvailableStrategy.Symbol}.txt"));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}