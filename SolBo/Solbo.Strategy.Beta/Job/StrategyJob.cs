using Kucoin.Net;
using Kucoin.Net.Interfaces;
using Kucoin.Net.Objects;
using Quartz;
using Solbo.Strategy.Beta.Models;
using Solbo.Strategy.Beta.Rules;
using Solbo.Strategy.Beta.Trading;
using Solbo.Strategy.Beta.Trading.Kucoin;
using Solbo.Strategy.Beta.Verificators.Storage;
using Solbo.Strategy.Beta.Verificators.Strategy;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solbo.Strategy.Beta.Job
{
    public class StrategyJob : IJob
    {
        private readonly IFileService _fileService;
        private readonly ILoggingService _loggingService;
        private IKucoinClient _kucoinClient;
        private ICollection<IBetaRule> _rules;
        public StrategyJob(
            IFileService fileService,
            ILoggingService loggingService)
        {
            _fileService = fileService;
            _loggingService = loggingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _rules = new HashSet<IBetaRule>();
            try
            {
                var strategyName = context.JobDetail.JobDataMap["name"] as string;
                var strategyPath = context.JobDetail.JobDataMap["path"] as string;
                var symbol = context.JobDetail.JobDataMap["symbol"] as string;
                var jobArgs = await _fileService.DeserializeAsync<StrategyRootModel>(strategyPath);
                var jobPerSymbol = jobArgs.Pairs.FirstOrDefault(j => j.Symbol == symbol);

                if (jobPerSymbol is null)
                    return;

                _kucoinClient = new KucoinClient(new KucoinClientOptions
                {
                    ApiCredentials = new KucoinApiCredentials(jobArgs.Exchange.Kucoin.ApiKey, jobArgs.Exchange.Kucoin.ApiSecret, jobArgs.Exchange.Kucoin.PassPhrase)
                });

                _rules.Add(new StrategyModelVerificator());
                _rules.Add(new ClearOnStartupVerificator(_fileService, context.PreviousFireTimeUtc, strategyName));

                _rules.Add(new KucoinSymbolRule(_kucoinClient));
                _rules.Add(new KucoinSymbolPriceRule(_kucoinClient));
                _rules.Add(new SavePriceRule(_fileService));
                _rules.Add(new AveragePriceRule(_fileService));
                _rules.Add(new CreateStorageRule(_fileService, strategyName));
                _rules.Add(new ProceedStopLossRule(_fileService, strategyName));

                _rules.Add(new AccountExchangeRule(_kucoinClient));

                if (jobPerSymbol.IsStopLossOn)
                {
                    _rules.Add(new StopLossStepRule());
                    _rules.Add(new StopLossPriceRule());
                    _rules.Add(new StopLossExecuteRule(_kucoinClient));
                }

                _loggingService.Info($"{context.JobDetail.Key.Name} - START JOB - TASKS ({_rules.Count})");

                foreach (var item in _rules)
                {
                    var result = item.Result(jobPerSymbol);
                    if (!result.Success)
                    {
                        _loggingService.Error($"{context.JobDetail.Key.Name}|{Environment.NewLine}{result.Message}");
                        break;
                    }
                    else
                    {
                        _loggingService.Info($"{context.JobDetail.Key.Name} - PROCEED TASK");
                    }
                }

                _loggingService.Info($"{context.JobDetail.Key.Name} - END JOB - TASKS ({_rules.Count})");
            }
            catch (JobExecutionException e)
            {
                _loggingService.Error($"{context.JobDetail.Key.Name}|{Environment.NewLine}Message => {e.Message}{Environment.NewLine}StackTrace => {e.StackTrace}");
            }
        }
    }
}