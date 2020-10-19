using Microsoft.Extensions.Configuration;
using NLog;
using Quartz;
using Quartz.Impl;
using SolBo.Agent.DI;
using SolBo.Agent.Factories;
using SolBo.Agent.Strategies;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services;
using System;
using System.IO;
using System.Security.Permissions;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolBo.Agent
{
    class Program
    {
        private static readonly string appId = "solbo-runtime";

        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        private static readonly Logger Logger = LogManager.GetLogger("SOLBO");

        private static IFileService _fileService;

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static async Task<int> Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            LogManager.Configuration.Variables["fileName"] = $"{appId}-{DateTime.UtcNow:ddMMyyyy}.log";
            LogManager.Configuration.Variables["archiveFileName"] = $"{appId}-{DateTime.UtcNow:ddMMyyyy}.log";

            _fileService = new FileService();

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

                #region Strategies
                if(app.Strategies.AnyAndNotNull())
                {
                    foreach (var strategy in app.Strategies)
                    {
                        if (string.Equals(strategy.Name, "BuyDeepSellHigh"))
                        {
                            var filePath = $"_{strategy.Name}.json";
                            var strategyDefinition = await _fileService.GetAsync<BuyDeepSellHighStrategy>(filePath);

                            if (strategyDefinition.Jobs.AnyAndNotNull())
                            {
                                foreach (var job in strategyDefinition.Jobs)
                                {
                                    if(job.IsActive)
                                    {
                                        var jobId = $"{strategy.Name}_{job.Id}";

                                        var jobDetail = JobBuilder.Create<BuyDeepSellHighJob>()
                                            .WithIdentity(jobId, strategy.Name)
                                            .Build();

                                        jobDetail.JobDataMap["args"] = JsonSerializer.Serialize(job);

                                        var jobBuilder = TriggerBuilder.Create()
                                             .WithIdentity($"{jobId}_t", strategy.Name)
                                             .WithSimpleSchedule(x => x
                                                .WithIntervalInMinutes(job.IntervalInMinutes)
                                                .RepeatForever())
                                             .StartNow();


                                        await _scheduler.ScheduleJob(jobDetail, jobBuilder.Build());
                                    }
                                }
                            }
                        }
                        if (string.Equals(strategy.Name, "RollingPrice"))
                        {
                            var filePath = $"_{strategy.Name}.json";
                            var strategyDefinition = await _fileService.GetAsync<RollingPriceStrategy>(filePath);

                            if (strategyDefinition.Jobs.AnyAndNotNull())
                            {
                                foreach (var job in strategyDefinition.Jobs)
                                {
                                    if(job.IsActive)
                                    {
                                        var jobId = $"{strategy.Name}_{job.Id}";

                                        var jobDetail = JobBuilder.Create<RollingPriceJob>()
                                            .WithIdentity(jobId, strategy.Name)
                                            .Build();

                                        jobDetail.JobDataMap["args"] = JsonSerializer.Serialize(job);

                                        var jobBuilder = TriggerBuilder.Create()
                                            .WithIdentity($"{jobId}_t", strategy.Name)
                                            .WithSimpleSchedule(x => x
                                                .WithIntervalInMinutes(job.IntervalInMinutes)
                                                .RepeatForever())
                                            .StartNow();

                                        await _scheduler.ScheduleJob(jobDetail, jobBuilder.Build());
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                Logger.Info($"Version: {app.Version}");

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