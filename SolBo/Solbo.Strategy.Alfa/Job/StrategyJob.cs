using Quartz;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Verificators;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solbo.Strategy.Alfa.Job
{
    [DisallowConcurrentExecution]
    public class StrategyJob : IJob
    {
        private readonly IFileService _fileService;
        private readonly ILoggingService _loggingService;

        private string _name;
        private readonly ICollection<IAlfaRule> _rules = new HashSet<IAlfaRule>();

        public StrategyJob(
            IFileService fileService,
            ILoggingService loggingService)
        {
            _fileService = fileService;
            _loggingService = loggingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _name = context.JobDetail.JobDataMap["name"] as string;
                var path = context.JobDetail.JobDataMap["path"] as string;
                var jobArgs = await _fileService.GetAsync<StrategyRootModel>(path);

                _rules.Add(new StrategyRootExchangeVerificator());
                _rules.Add(new StrategyExchangeBinanceVerificator());

                foreach (var item in _rules)
                {
                    var result = item.Result(jobArgs);

                    if (!result.Success)
                    {
                        _loggingService.Error($"{_name} - {result.Message}"); break;
                    }
                }
                _loggingService.Info($"{_name} - END JOB | RULES - {_rules.Count}");
                _rules.Clear();
            }
            catch (JobExecutionException e)
            {
                _loggingService.Error($"{_name} => Message => {e.Message}{Environment.NewLine} StackTrace => {e.StackTrace}");
            }
        }
    }
}