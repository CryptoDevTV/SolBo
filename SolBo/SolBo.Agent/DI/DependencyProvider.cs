using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolBo.Agent.DI
{
    public class DependencyProvider
    {
        public static IServiceProvider Get(App app, List<PluginLoader> loaders)
        {
            var services = new ServiceCollection();

            #region Plugins
            foreach (var loader in loaders)
            {
                foreach (var pluginType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IStrategyPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    var plugin = Activator.CreateInstance(pluginType) as IStrategyPlugin;

                    plugin?.Configure(services);
                }
            }
            #endregion

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

            #region Exchanges
            //services.AddTransient<IBinanceClient, BinanceClient>();
            //if (app.Exchange.Type == ExchangeType.Binance && !app.Exchange.IsInTestMode)
            //{
            //    services.AddTransient<IBinanceClient>(s => new BinanceClient(new BinanceClientOptions
            //    {
            //        ApiCredentials = new ApiCredentials(app.Exchange.ApiKey, app.Exchange.ApiSecret)
            //    }));
            //}
            //services.AddTransient<IKucoinClient, KucoinClient>();
            //if (app.Exchange.Type == ExchangeType.KuCoin && !app.Exchange.IsInTestMode)
            //{
            //    services.AddTransient<IKucoinClient>(s => new KucoinClient(new KucoinClientOptions
            //    {
            //        ApiCredentials = new KucoinApiCredentials(app.Exchange.ApiKey, app.Exchange.ApiSecret, app.Exchange.PassPhrase)
            //    }));
            //}
            #endregion

            #region Services
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ILoggingService, LoggingService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}