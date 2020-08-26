using Microsoft.Extensions.Configuration;
using NLog;
using Quartz;
using Quartz.Impl;
using SolBo.Agent.DI;
using SolBo.Agent.Factories;
using SolBo.Agent.Jobs;
using SolBo.Shared.Domain.Configs;
using System;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace SolBo.Agent
{
    class Program
    {
        private static readonly string appId = "solbo-runtime";

        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static async Task<int> Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            LogManager.Configuration.Variables["fileName"] = $"{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";
            LogManager.Configuration.Variables["archiveFileName"] = $"{appId}-{DateTime.UtcNow.ToString("ddMMyyyy")}.log";

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

                #region Buy Deep Sell High
                IJobDetail bdshJob = JobBuilder.Create<BuyDeepSellHighJob>()
                    .WithIdentity("BuyDeepSellHighJob")
                    .Build();

                bdshJob.JobDataMap["FileName"] = app.FileName;
                bdshJob.JobDataMap["Version"] = app.Version;

                var bdshBuilder = TriggerBuilder.Create()
                    .WithIdentity("BuyDeepSellHighJobTrigger")
                    .StartNow();

                bdshBuilder.WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(app.IntervalInMinutes)
                        .RepeatForever());

                var bdshTrigger = bdshBuilder.Build();
                #endregion

                await _scheduler.ScheduleJob(bdshJob, bdshTrigger);

                await Task.Delay(TimeSpan.FromSeconds(30));

                Console.Read();
            }
            catch (SchedulerException ex)
            {
                Logger.Fatal($"{ex.Message}");
            }

            LogManager.Shutdown();

            return 0;
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Logger.Fatal($"{e.Message}");
            Logger.Fatal($"{args.IsTerminating}");
        }
    }
}