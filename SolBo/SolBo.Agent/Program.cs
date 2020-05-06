using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using SolBo.Agent.DI;
using SolBo.Agent.Factories;
using SolBo.Agent.Jobs;
using SolBo.Shared.Domain.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolBo.Agent
{
    class Program
    {
        private static readonly string appId = "solbo";

        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        static async Task<int> Main()
        {
            NLog.LogManager.Configuration.Variables["fileName"] = $"{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
            NLog.LogManager.Configuration.Variables["archiveFileName"] = $"{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

            var cfgBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{appId}.json");

            var configuration = cfgBuilder.Build();

            var app = configuration.Get<App>();

            try
            {
                var servicesProvider = DependencyProvider.Get(app);

                var jobFactory = new JobFactory(servicesProvider);

                _schedulerFactory = new StdSchedulerFactory();

                _scheduler = await _schedulerFactory.GetScheduler();

                _scheduler.JobFactory = jobFactory;

                await _scheduler.Start();

                #region Exchange Configuration
                var exchange = (app.Exchanges as IList<Exchange>).FirstOrDefault();

                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(exchange.ApiKey, exchange.ApiSecret)
                });
                #endregion

                #region Buy Deep Sell High
                IJobDetail bdshJob = JobBuilder.Create<BuyDeepSellHighJob>()
                    .WithIdentity("BuyDeepSellHighJob")
                    .Build();

                bdshJob.JobDataMap["Strategy"] = app.Strategy;

                var bdshBuilder = TriggerBuilder.Create()
                    .WithIdentity("BuyDeepSellHighJobTrigger")
                    .StartNow();

                bdshBuilder.WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(app.Strategy.IntervalInMinutes)
                        .RepeatForever());

                var bdshTrigger = bdshBuilder.Build();
                #endregion

                await _scheduler.ScheduleJob(bdshJob, bdshTrigger);

                await Task.Delay(TimeSpan.FromSeconds(30));

                Console.ReadKey();
            }
            catch (SchedulerException)
            {

            }

            NLog.LogManager.Shutdown();

            return 0;
        }
    }
}