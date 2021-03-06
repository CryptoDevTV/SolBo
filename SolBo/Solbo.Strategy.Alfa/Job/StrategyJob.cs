using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using Quartz;
using Solbo.Strategy.Alfa.Models;
using Solbo.Strategy.Alfa.Rules;
using Solbo.Strategy.Alfa.Trading;
using Solbo.Strategy.Alfa.Verificators.Storage;
using Solbo.Strategy.Alfa.Verificators.Strategy;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solbo.Strategy.Alfa.Job
{
    //[DisallowConcurrentExecution]
    public class StrategyJob : IJob
    {
        private readonly IFileService _fileService;
        private readonly ILoggingService _loggingService;
        
        private IBinanceClient _binanceClient;
        private ICollection<IAlfaRule> _rules;
        public StrategyJob(
            IFileService fileService,
            ILoggingService loggingService)
        {
            _fileService = fileService;
            _loggingService = loggingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _rules = new HashSet<IAlfaRule>();
            try
            {
                var strategyName = context.JobDetail.JobDataMap["name"] as string;
                var strategyPath = context.JobDetail.JobDataMap["path"] as string;
                var symbol = context.JobDetail.JobDataMap["symbol"] as string;
                var jobArgs = await _fileService.DeserializeAsync<StrategyRootModel>(strategyPath);
                var jobPerSymbol = jobArgs.Pairs.FirstOrDefault(j => j.Symbol == symbol);

                if (jobPerSymbol is null)
                    return;

                _binanceClient = new BinanceClient(new BinanceClientOptions
                {
                    ApiCredentials = new ApiCredentials(jobArgs.Exchange.Binance.ApiKey, jobArgs.Exchange.Binance.ApiSecret)
                });

                _rules.Add(new StrategyModelVerificator());
                _rules.Add(new ClearOnStartupVerificator(_fileService, context.PreviousFireTimeUtc, strategyName));

                _rules.Add(new BinanceSymbolRule(_binanceClient));
                _rules.Add(new BinanceSymbolPriceRule(_binanceClient));
                _rules.Add(new SavePriceRule(_fileService, strategyName));
                _rules.Add(new AveragePriceRule(_fileService));
                _rules.Add(new CreateStorageRule(_fileService, strategyName));

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