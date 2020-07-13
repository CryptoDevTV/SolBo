using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SolBo.Agent.Jobs;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using SolBo.Shared.Services.Implementations;
using System;

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
            services.AddTransient<IStorageService, FileStorageService>();

            services.AddTransient<IMarketService, MarketService>();

            services.AddTransient<IConfigurationService, ConfigurationService>();
            #endregion

            #region Notifications
            services.AddTransient<IPushOverNotificationService>(
                s => new PushOverNotificationService(
                    app.Notifications.Pushover.Token,
                    app.Notifications.Pushover.Recipients,
                    app.Notifications.Pushover.Endpoint,
                    app.Notifications.Pushover.IsActive));
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}