using Autofac;
using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Engines;
using Surging.Core.CPlatform.Module;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Runtime.Client;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Support;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.ServiceHosting.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform
{
    /// <summary>
    /// 微服务主机扩展类
    /// </summary>
    public static class ServiceHostBuilderExtensions
    {
        /// <summary>
        /// 指定宿主机环境
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IServiceHostBuilder UseServer(this IServiceHostBuilder hostBuilder, string address, int port, string token = "True")
        {
            return hostBuilder.MapServices(mapper =>
            {
                BuildServiceEngine(mapper);
                mapper.Resolve<IServiceCommandManager>().SetServiceCommandsAsync();
                var serviceToken = mapper.Resolve<IServiceTokenGenerator>().GeneratorToken(token);
                var _port = AppConfig.ServerOptions.Port == 0 ? port : AppConfig.ServerOptions.Port;
                var _address = AppConfig.ServerOptions.Ip ?? address;
                var _ip = AddressHelper.GetIpFromAddress(_address);

                _port = AppConfig.ServerOptions.IpEndpoint?.Port ?? _port;
                _ip = AppConfig.ServerOptions.IpEndpoint?.Address.ToString() ?? _ip;
                if (_ip.IndexOf(".") < 0 || _ip == "" || _ip == "0.0.0.0")
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && (_ip == "" || _ip == "0.0.0.0" || _ip == adapter.Name))
                        {
                            IPInterfaceProperties ipxx = adapter.GetIPProperties();
                            UnicastIPAddressInformationCollection ipCollection = ipxx.UnicastAddresses;
                            foreach (UnicastIPAddressInformation ipadd in ipCollection)
                            {
                                if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    _ip = ipadd.Address.ToString();
                                }
                            }
                        }
                    }
                }
                var mappingIp = AppConfig.ServerOptions.MappingIP ?? _ip;
                var mappingPort = AppConfig.ServerOptions.MappingPort;
                if (mappingPort == 0)
                    mappingPort = _port;

                if (string.IsNullOrEmpty(AppConfig.ServerOptions.MappingIP))
                {
                    ConfigureRoute(mapper, _ip, _port, serviceToken);
                }
                else
                {
                    ConfigureRoute(mapper, _ip, _port, mappingIp, mappingPort, serviceToken);
                }
                //初始化模块
                mapper.Resolve<IModuleProvider>().Initialize();
                //获取微服务主机(可能存在多个运行时主机,取决于微服务依赖的模块)
                var serviceHosts = mapper.Resolve<IList<Runtime.Server.IServiceHost>>();
                Task.Factory.StartNew(async () =>
                {
                    foreach (var serviceHost in serviceHosts)
                        await serviceHost.StartAsync(_ip, _port);
                    mapper.Resolve<IServiceEngineLifetime>().NotifyStarted();
                }).Wait();
            });
        }

        public static IServiceHostBuilder UseServer(this IServiceHostBuilder hostBuilder, Action<SurgingServerOptions> options)
        {
            var serverOptions = new SurgingServerOptions();
            options.Invoke(serverOptions);
            AppConfig.ServerOptions = serverOptions;
            return hostBuilder.UseServer(serverOptions.Ip, serverOptions.Port, serverOptions.Token);
        }

        public static IServiceHostBuilder UseClient(this IServiceHostBuilder hostBuilder)
        {
            return hostBuilder.MapServices(mapper =>
            {
                var serviceEntryManager = mapper.Resolve<IServiceEntryManager>();
                var addressDescriptors = serviceEntryManager.GetEntries().Select(i =>
                {
                    i.Descriptor.Metadatas = null;
                    if (!string.IsNullOrEmpty(AppConfig.ServerOptions.MappingIP))
                    {
                        return new ServiceSubscriber
                        {
                            Address = new[] {
                            new MappingAddressModel {
                                 InnerIp = Dns.GetHostEntry(Dns.GetHostName())
                                 .AddressList.FirstOrDefault<IPAddress>(a => a.AddressFamily.ToString().Equals("InterNetwork")).ToString(),
                                 //MappingIp = AppConfig.ServerOptions.MappingIP,
                                }
                            },
                            ServiceDescriptor = i.Descriptor
                        };
                    }
                    else
                    {
                        return new ServiceSubscriber
                        {
                            Address = new[] {
                            new IpAddressModel {
                                Ip = Dns.GetHostEntry(Dns.GetHostName())
                                .AddressList.FirstOrDefault<IPAddress>(a => a.AddressFamily.ToString().Equals("InterNetwork")).ToString()
                                }
                            },
                            ServiceDescriptor = i.Descriptor
                        };
                    }
                }).ToList();
                mapper.Resolve<IServiceSubscribeManager>().SetSubscribersAsync(addressDescriptors);
                mapper.Resolve<IModuleProvider>().Initialize();
            });
        }

        /// <summary>
        /// 构建服务引擎
        /// </summary>
        /// <param name="container"></param>
        public static void BuildServiceEngine(IContainer container)
        {
            if (container.IsRegistered<IServiceEngine>())
            {
                var builder = new ContainerBuilder();

                container.Resolve<IServiceEngineBuilder>().Build(builder);
                var configBuilder = container.Resolve<IConfigurationBuilder>();
                var appSettingPath = Path.Combine(AppConfig.ServerOptions.RootPath, "appsettings.json");
                configBuilder.AddCPlatformFile("${appsettingspath}|" + appSettingPath, optional: false, reloadOnChange: true);
                builder.Update(container);
            }
        }

        public static void ConfigureRoute(IContainer mapper, string mappingIp, int mappingPort, string serviceToken)
        {
            var serviceEntryManager = mapper.Resolve<IServiceEntryManager>();
            if (AppConfig.ServerOptions.Protocol == CommunicationProtocol.Tcp || AppConfig.ServerOptions.Protocol == CommunicationProtocol.None)
                new ServiceRouteWatch(mapper.Resolve<CPlatformContainer>(), () =>
                {
                    var addess = new IpAddressModel
                    {
                        Ip = mappingIp,
                        Port = mappingPort,
                        ProcessorTime = Math.Round(Convert.ToDecimal(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds), 2, MidpointRounding.AwayFromZero),
                    };
                    RpcContext.GetContext().SetAttachment("Host", addess);

                    var addressDescriptors = serviceEntryManager.GetEntries().Select(i =>
                    {
                        i.Descriptor.Token = serviceToken;
                        return new ServiceRoute
                        {
                            Address = new[] { addess },
                            ServiceDescriptor = i.Descriptor
                        };
                    }).ToList();
                    mapper.Resolve<IServiceRouteManager>().SetRoutesAsync(addressDescriptors).Wait();
                });
        }

        public static void ConfigureRoute(IContainer mapper, string ip, int port, string mappingIp, int mappingPort, string serviceToken)
        {
            var serviceEntryManager = mapper.Resolve<IServiceEntryManager>();
            if (AppConfig.ServerOptions.Protocol == CommunicationProtocol.Tcp || AppConfig.ServerOptions.Protocol == CommunicationProtocol.None)
            {
                new ServiceRouteWatch(mapper.Resolve<CPlatformContainer>(), () =>
                {
                    var addess = new MappingAddressModel(ip, port, mappingIp, mappingPort);
                    RpcContext.GetContext().SetAttachment("Host", addess);

                    var addressDescriptors = serviceEntryManager.GetEntries().Select(i =>
                    {
                        i.Descriptor.Token = serviceToken;
                        return new ServiceRoute
                        {
                            Address = new[] { addess },
                            ServiceDescriptor = i.Descriptor
                        };
                    }).ToList();
                    mapper.Resolve<IServiceRouteManager>().SetRoutesAsync(addressDescriptors).Wait();
                });
            }
        }
    }
}