using Binance.Net.Interfaces;
using Kucoin.Net.Interfaces;
using NLog;
using Quartz;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using SolBo.Shared.Extensions;
using SolBo.Shared.Rules;
using SolBo.Shared.Rules.Mode;
using SolBo.Shared.Rules.Sequence;
using SolBo.Shared.Rules.Validation;
using SolBo.Shared.Rules.Validation.Generated;
using SolBo.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Agent.Strategies
{
    [DisallowConcurrentExecution]
    public class BuyDeepSellHighJob : IJob
    {
        public StrategyType StrategiesType => StrategyType.BUY_DEEP_SELL_HIGH;
        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        private readonly IBinanceClient _binanceClient;
        private readonly IKucoinClient _kucoinClient;

        //private readonly IStorageService _storageService;
        //private readonly IMarketService _marketService;
        //private readonly IConfigurationService _schedulerService;
        //private readonly IPushOverNotificationService _pushOverNotificationService;
        //private readonly IBinanceTickerService _binanceTickerService;
        //private readonly IKucoinTickerService _kucoinTickerService;

        private readonly ICollection<IRule> _rules = new HashSet<IRule>();

        public BuyDeepSellHighJob(
            IBinanceClient binanceClient,
            IKucoinClient kucoinClient)
        {
            _binanceClient = binanceClient;
            _kucoinClient = kucoinClient;
        }

        //public BuyDeepSellHighJob(
        //    IBinanceClient binanceClient,
        //    IKucoinClient kucoinClient,
        //    IStorageService storageService,
        //    IMarketService marketService,
        //    IConfigurationService schedulerService,
        //    IPushOverNotificationService pushOverNotificationService,
        //    IBinanceTickerService binanceTickerService,
        //    IKucoinTickerService kucoinTickerService)
        //{
        //    _binanceClient = binanceClient;
        //    _kucoinClient = kucoinClient;
        //    _storageService = storageService;
        //    _marketService = marketService;
        //    _schedulerService = schedulerService;
        //    _pushOverNotificationService = pushOverNotificationService;
        //    _binanceTickerService = binanceTickerService;
        //    _kucoinTickerService = kucoinTickerService;
        //}

        public async Task Execute(IJobExecutionContext context)
        {
            var jobArgs = context.JobDetail.JobDataMap["args"] as string;
            var job = JsonSerializer.Deserialize<BuyDeepSellHigh>(jobArgs);

            var botArgs = context.JobDetail.JobDataMap["exchange"] as string;
            var exchange = JsonSerializer.Deserialize<Exchange>(botArgs);
            try
            {
                Logger.Info($"BDSH - {job.Id} - {job.Symbol} - {job.SellType.GetDescription()} on {exchange.Type.GetDescription()}");

                //if (readConfig.ReadSucces)
                //{
                //    var solbot = readConfig.SolBotConfig;

                //    _rules.Add(new SendNotificationRule(_pushOverNotificationService, context.PreviousFireTimeUtc));

                //    _rules.Add(new StrategyValidationRule());
                //    _rules.Add(new ModeTypeValidationRule());
                //    _rules.Add(new AverageValidationRule());
                //    _rules.Add(new AverageTypeValidationRule());
                //    _rules.Add(new SellTypeValidationRule());
                //    _rules.Add(new BuyStepValidationRule());
                //    _rules.Add(new SellStepValidationRule());
                //    _rules.Add(new CommissionTypeValidationRule());
                //    _rules.Add(new StopLossStepValidationRule());
                //    _rules.Add(new StopLossPauseCyclesValidationRules());
                //    _rules.Add(new FundStepValidationRule());
                //    _rules.Add(new BoughtValidationRule());
                //    _rules.Add(new StopLossCurrentCycleValidationRule());
                //    _rules.Add(new SetStorageSequenceRule(_storageService));
                //    _rules.Add(new ClearOnStartupSequenceRule(_storageService, context.PreviousFireTimeUtc));

                //    _rules.Add(new SwitchExchangeForSymbolRule(
                //        selectedExchange,
                //        _binanceClient,
                //        _kucoinClient,
                //        _binanceTickerService,
                //        _kucoinTickerService));

                //    _rules.Add(new SavePriceSequenceRule(_storageService));
                //    _rules.Add(new AverageTypeSequenceRule());
                //    _rules.Add(new CalculateAverageSequenceRule(_storageService, _marketService));

                //    _rules.Add(new ModeTypeSequenceRule());
                //    _rules.Add(new PumpStopLossCycleSequenceRule());

                //    if (selectedExchange.IsInTestMode)
                //        _rules.Add(new ModeTestRule(_marketService, _pushOverNotificationService));
                //    else
                //    {
                //        _rules.Add(new ApiCredentialsValidationRule(selectedExchange));
                //        _rules.Add(new SwitchExchangeForProductionRule(
                //            _marketService,
                //            selectedExchange,
                //            _binanceClient,
                //            _kucoinClient,
                //            _pushOverNotificationService));
                //    }

                //    foreach (var item in _rules)
                //    {
                //        var result = item.RuleExecuted(solbot);

                //        if (result.Success)
                //            Logger.Trace($"{result.Message}");
                //        else
                //        {
                //            Logger.Error($"{result.Message}");

                //            break;
                //        }
                //    }

                //    var saveConfig = await _schedulerService.SetConfigAsync(configFileName, solbot);

                //    if (saveConfig.WriteSuccess)
                //        Logger.Info(LogGenerator.SaveSuccess(botVersion));
                //    else
                //        Logger.Error(LogGenerator.SaveError(botVersion));
                //}
            }
            catch (Exception e)
            {
                Logger.Fatal($"{Environment.NewLine}[{StrategiesType.GetDescription()}] Message => {e.Message}{Environment.NewLine} StackTrace => {e.StackTrace}");
            }
        }
    }
}