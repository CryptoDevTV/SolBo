using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SolBo.Shared.Services;
using SolBo.Shared.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolBo.Agent.DI
{
    public class DependencyProvider
    {
        public static IServiceProvider Get(List<PluginLoader> loaders)
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

            #region Services
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ILoggingService, LoggingService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}