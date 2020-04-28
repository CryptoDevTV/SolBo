using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SolBo.Agent.Jobs;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using SolBo.Shared.Services.Implementations;
using System;
using System.Linq;

namespace SolBo.Agent.DI
{
    public class DependencyProvider
    {
        public static IServiceProvider Get(App app)
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
            var selectedStrategy = app.Strategy.Available
                .FirstOrDefault(s => s.Id == app.Strategy.ActiveId);

            services.AddTransient<IStorageService>(
                s => new FileStorageService(selectedStrategy.StoragePath)
                );

            services.AddTransient<IMarketService, MarketService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}