using Autofac;
using Microsoft.Extensions.Logging;
using Surging.Core.Caching.Configurations;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using System;

namespace Hl.ServiceHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHostBuilder()
                 .RegisterServices(builder =>
                 { 
                     builder.AddMicroService(option =>
                      {
                          option.AddServiceRuntime()
                           .AddClientProxy()
                           .AddRelateServiceRuntime()
                           .AddConfigurationWatch()
                           .AddServiceEngine(typeof(SurgingServiceEngine));
                          //.AddDapperRepository();
                          builder.Register(p => new CPlatformContainer(ServiceLocator.Current));
                      });
                 })
                 .ConfigureLogging(loggging =>
                 {
                     loggging.AddConfiguration(
                         AppConfig.GetSection("Logging"));
                 })
                 .UseServer(options => { })
                 .UseConsoleLifetime()
                 .Configure(build =>
                 {
                     build.AddCacheFile("${cachepath}|Configs/cacheSettings.json", optional: false, reloadOnChange: true);
                     build.AddCPlatformFile("${srcppath}|Configs/srcpSettings.json", optional: false, reloadOnChange: true);
                     build.AddEventBusFile("Configs/eventBusSettings.json", optional: false);
                     build.AddConsulFile("Configs/consul.json", optional: false, reloadOnChange: true);
                 })
                 .UseProxy()
                 .UseStartup<Startup>()
                 .Build();
        }
    }
}