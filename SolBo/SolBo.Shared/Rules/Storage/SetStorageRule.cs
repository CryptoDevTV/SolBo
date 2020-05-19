using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
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
        public string Message { get; set; }
        public string StoragePath { get; set; }
        public ResultRule ExecutedRule(Solbot solbot)
        {
            var result = RulePassed(solbot);

            return new ResultRule
            {
                Success = result,
                Message = result
                    ? $"{RuleName} SUCCESS => Storage Path: {StoragePath}."
                    : $"{RuleName} error. {Message}"
            };
        }
        public bool RulePassed(Solbot solbot)
        {
            try
            {
                StoragePath = Path.Combine(
                    solbot.Strategy.AvailableStrategy.StoragePath,
                    $"{solbot.Strategy.AvailableStrategy.Symbol}.txt");

                _storageService.SetPath(StoragePath);

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