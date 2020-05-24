using Binance.Net;
using NLog;
using Quartz;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Rules;
using SolBo.Shared.Rules.Mode;
using SolBo.Shared.Rules.Sequence;
using SolBo.Shared.Rules.Validation;
using SolBo.Shared.Rules.Validation.Generated;
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
        private readonly IConfigurationService _schedulerService;

        private readonly ICollection<IRule> _rules = new HashSet<IRule>();

        public BuyDeepSellHighJob(
            IStorageService storageService,
            IMarketService marketService,
            IConfigurationService schedulerService)
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

                    _rules.Add(new StrategyValidationRule());
                    _rules.Add(new ModeTypeValidationRule());
                    _rules.Add(new TickerValidationRule());
                    _rules.Add(new AverageValidationRule());
                    _rules.Add(new BuyStepValidationRule());
                    _rules.Add(new SellStepValidationRule());
                    _rules.Add(new StopLossStepValidationRule());
                    _rules.Add(new StopLossTypeValidationRule());
                    _rules.Add(new FundStepValidationRule());
                    _rules.Add(new BoughtValidationRule());

                    _rules.Add(new SetStorageSequenceRule(_storageService));

                    using (var client = new BinanceClient())
                    {
                        _rules.Add(new SymbolSequenceRule(client));
                        _rules.Add(new GetPriceSequenceRule(client));
                        _rules.Add(new SavePriceSequenceRule(_storageService));
                        _rules.Add(new CalculateAverageSequenceRule(_storageService));

                        _rules.Add(new ModeTypeSequenceRule());

                        if (solbot.Exchange.IsInTestMode)
                            _rules.Add(new ModeTestRule(_marketService));
                        else
                            _rules.Add(new ModeProductionRule(_marketService));

                        foreach (var item in _rules)
                        {
                            var result = item.RuleExecuted(solbot);

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
                        Logger.Trace(LogGenerator.SaveSuccess);
                    else
                        Logger.Error(LogGenerator.SaveError);
                }
            }
            catch (Exception e)
            {
                Logger.Fatal($"{e.Message}");
            }
        }
    }
}