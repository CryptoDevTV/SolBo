using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Quartz;
using Quartz.Impl;
using SolBo.Agent.DI;
using SolBo.Agent.Factories;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
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
            var cancellationTokenSource = new CancellationTokenSource();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            AppDomain.CurrentDomain.ProcessExit += (s, e) => cancellationTokenSource.Cancel();
            Console.CancelKeyPress += (s, e) => cancellationTokenSource.Cancel();

            LogManager.Configuration.Variables["fileName"] = $"{appId}-{DateTime.UtcNow:ddMMyyyy}.log";
            LogManager.Configuration.Variables["archiveFileName"] = $"{appId}-{DateTime.UtcNow:ddMMyyyy}.log";

            var cfgBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{appId}.json");

            var configuration = cfgBuilder.Build();

            var app = configuration.Get<App>();

            Logger.Info($"Version: {app.Version}");

            try
            {
                var loaders = GetPluginLoaders();

                var servicesProvider = DependencyProvider.Get(app, loaders);

                var jobFactory = new JobFactory(servicesProvider);

                _schedulerFactory = new StdSchedulerFactory();

                _scheduler = await _schedulerFactory.GetScheduler();

                _scheduler.JobFactory = jobFactory;

                await _scheduler.Start();

                foreach (var loader in loaders)
                {
                    foreach (var pluginType in loader
                        .LoadDefaultAssembly()
                        .GetTypes()
                        .Where(t => typeof(IStrategyPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        var strategy = Activator.CreateInstance(pluginType) as IStrategyPlugin;

                        var strategyDefined = app.Strategies.FirstOrDefault(s => s.Name == strategy?.Name());

                        if (!(strategyDefined is null))
                        {
                            foreach (var pair in strategyDefined.Pairs)
                            {
                                var runtime = strategy?.StrategyRuntime(pair);

                                switch (pair.IntervalType)
                                {
                                    case IntervalType.ONETIME:
                                        {
                                            runtime.Item2.StartNow();
                                        }
                                        break;
                                    case IntervalType.SECONDS:
                                        {
                                            runtime.Item2.WithSimpleSchedule(x => x
                                                .WithIntervalInSeconds(pair.Interval)
                                                .RepeatForever());
                                        }
                                        break;
                                    case IntervalType.MINUTES:
                                        {
                                            runtime.Item2.WithSimpleSchedule(x => x
                                                .WithIntervalInMinutes(pair.Interval)
                                                .RepeatForever());
                                        }
                                        break;
                                    case IntervalType.HOURS:
                                        {
                                            runtime.Item2.WithSimpleSchedule(x => x
                                                .WithIntervalInHours(pair.Interval)
                                                .RepeatForever());
                                        }
                                        break;
                                }

                                await Task.Delay(TimeSpan.FromMilliseconds(500));
                                await _scheduler.ScheduleJob(runtime.Item1, runtime.Item2.Build());
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30));

                await Task.Delay(-1, cancellationTokenSource.Token).ContinueWith(t => { });
            }
            catch (SchedulerException ex)
            {
                Logger.Fatal($"{ex.Message}");
            }

            await _scheduler.Shutdown();

            LogManager.Shutdown();

            return 0;
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Logger.Fatal($"{e.Message}");
            Logger.Fatal($"{args.IsTerminating}");
        }

        private static List<PluginLoader> GetPluginLoaders()
        {
            var loaders = new List<PluginLoader>();

            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "strategies");
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, $"Solbo.Strategy.{dirName}.dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new[] { typeof(IStrategyPlugin), typeof(IServiceCollection) });
                    loaders.Add(loader);
                }
            }

            return loaders;
        }
    }
}