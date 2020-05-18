using Binance.Net;
using NLog;
using Quartz;
using SolBo.Shared.Rules;
using SolBo.Shared.Rules.Market;
using SolBo.Shared.Rules.Mode;
using SolBo.Shared.Rules.Online;
using SolBo.Shared.Rules.Storage;
using SolBo.Shared.Rules.Strategy;
using SolBo.Shared.Rules.Validation;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolBo.Agent.Jobs
{
    [DisallowConcurrentExecution]
    public class BuyDeepSellHighJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        private readonly IStorageService _storageService;
        private readonly IMarketService _marketService;
        private readonly ISchedulerService _schedulerService;

        private readonly ICollection<IRule> _rules = new HashSet<IRule>();

        public BuyDeepSellHighJob(
            IStorageService storageService,
            IMarketService marketService,
            ISchedulerService schedulerService)
        {
            _storageService = storageService;
            _marketService = marketService;
            _schedulerService = schedulerService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var configFileName = context.JobDetail.JobDataMap["FileName"] as string;

                var readConfig = await _schedulerService.GetConfigAsync(configFileName);

                if (readConfig.ReadSucces)
                {
                    var solbot = readConfig.SolBotConfig;

                    _rules.Add(new StrategyRule());

                    _rules.Add(new StoragePathValidationRule());
                    _rules.Add(new TickerValidationRule());
                    _rules.Add(new AverageValidationRule());
                    _rules.Add(new BuyStepValidationRule());
                    _rules.Add(new SellStepValidationRule());
                    _rules.Add(new StopLossStepValidationRule());
                    _rules.Add(new StopLossTypeValidationRule());
                    _rules.Add(new FundStepValidationRule());
                    _rules.Add(new ActionsRule());

                    _rules.Add(new SetStorageRule(_storageService));

                    using (var client = new BinanceClient())
                    {
                        _rules.Add(new SymbolRule(client));
                        _rules.Add(new GetPriceRule(client));
                        _rules.Add(new SavePriceRule(_storageService));
                        _rules.Add(new CalculateAverageRule(_storageService));

                        if (solbot.Exchange.IsInTestMode)
                            _rules.Add(new ModeTestRule(_marketService));
                        else
                            _rules.Add(new ModeProductionRule(_marketService));

                        foreach (var item in _rules)
                        {
                            var result = item.ExecutedRule(solbot);

                            if (result.Success)
                                Logger.Trace($"{result.Message}");
                            else
                            {
                                Logger.Error($"{result.Message}");

                                break;
                            }
                        }
                    }

                    var saveConfig = await _schedulerService.SetConfigAsync(configFileName, solbot);

                    if (saveConfig.WriteSuccess)
                        Logger.Trace($"Save config success");
                    else
                        Logger.Error($"Save config error");
                }
            }
            catch (Exception e)
            {
                Logger.Fatal($"{e.Message}");
            }
        }
    }
}