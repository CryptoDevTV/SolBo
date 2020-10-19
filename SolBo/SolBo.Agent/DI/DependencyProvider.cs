using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using Kucoin.Net;
using Kucoin.Net.Interfaces;
using Kucoin.Net.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SolBo.Agent.Strategies;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
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

            #region Strategies
            services.AddTransient<BuyDeepSellHighJob>();
            services.AddTransient<RollingPriceJob>();
            #endregion

            #region Exchanges
            services.AddTransient<IBinanceClient, BinanceClient>();
            if (app.Exchange.Type == ExchangeType.Binance && !app.Exchange.IsInTestMode)
            {
                services.AddTransient<IBinanceClient>(s => new BinanceClient(new BinanceClientOptions
                {
                    ApiCredentials = new ApiCredentials(app.Exchange.ApiKey, app.Exchange.ApiSecret)
                }));
            }
            services.AddTransient<IKucoinClient, KucoinClient>();
            if (app.Exchange.Type == ExchangeType.KuCoin && !app.Exchange.IsInTestMode)
            {
                services.AddTransient<IKucoinClient>(s => new KucoinClient(new KucoinClientOptions
                {
                    ApiCredentials = new KucoinApiCredentials(app.Exchange.ApiKey, app.Exchange.ApiSecret, app.Exchange.PassPhrase)
                }));
            }
            #endregion

            #region Services
            services.AddTransient<IBinanceTickerService, BinanceTickerService>();
            services.AddTransient<IKucoinTickerService, KucoinTickerService>();
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