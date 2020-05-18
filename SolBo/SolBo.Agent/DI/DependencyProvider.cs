using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SolBo.Agent.Jobs;
using SolBo.Shared.Services;
using SolBo.Shared.Services.Implementations;
using System;

namespace SolBo.Agent.DI
{
    public class DependencyProvider
    {
        public static IServiceProvider Get()
        {

            var services = new ServiceCollection();

            #region Logging
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });
            #endregion

            #region Jobs
            services.AddTransient<BuyDeepSellHighJob>();
            #endregion

            #region Services
            services.AddTransient<IStorageService, FileStorageService>();

            services.AddTransient<IMarketService, MarketService>();

            services.AddTransient<ISchedulerService, SchedulerService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}