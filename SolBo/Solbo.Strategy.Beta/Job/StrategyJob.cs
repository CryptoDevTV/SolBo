using Quartz;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Verificators.Exchange;
using Solbo.Strategy.Beta.Verificators.Strategy;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solbo.Strategy.Beta.Job
{
    [DisallowConcurrentExecution]
    public class StrategyJob : IJob
    {
        private readonly IFileService _fileService;
        private readonly ILoggingService _loggingService;

        private string _name;
        private ICollection<IBetaRules> _rules;

        public StrategyJob(
            IFileService fileService,
            ILoggingService loggingService)
        {
            _fileService = fileService;
            _loggingService = loggingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _rules = new HashSet<IBetaRules>();
            try
            {
                _name = context.JobDetail.JobDataMap["name"] as string;
                var path = context.JobDetail.JobDataMap["path"] as string;
                var jobArgs = await _fileService.GetAsync<StrategyRootModel>(path);
                var symbol = context.JobDetail.JobDataMap["symbol"] as string;
                var jobPerSymbol = jobArgs.Pairs.FirstOrDefault(j => j.Symbol == symbol);

                if (jobPerSymbol is null)
                    return;

                _rules.Add(new StrategyRootExchangeVerificator());
                _rules.Add(new StrategyExchangeKucoinVerificator());
                _rules.Add(new StrategyModelVerificator());

                foreach (var item in _rules)
                {
                    var result = item.Result(jobArgs);

                    if (!result.Success)
                        _loggingService.Error($"{_name} - {result.Message}"); break;
                }
                _loggingService.Info($"{_name}({jobPerSymbol.Symbol} - {jobPerSymbol.Text}) - END JOB | RULES - {_rules.Count}");
            }
            catch (JobExecutionException e)
            {
                _loggingService.Error($"{_name} => Message => {e.Message}{Environment.NewLine} StackTrace => {e.StackTrace}");
            }
        }
    }
}