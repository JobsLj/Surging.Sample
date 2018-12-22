using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Surging.Core.ServiceHosting.Internal.Implementation
{
    /// <summary>
    /// 默认的主机构建者
    /// </summary>
    public class ServiceHostBuilder : IServiceHostBuilder
    {
        /// <summary>
        /// IServiceCollection 默认的依赖注入(DI)委托,是Asp.net Core的默认容器,通过这个委托可以为IServiceCollection注入相关的组件
        /// </summary>
        private readonly List<Action<IServiceCollection>> _configureServicesDelegates;

        /// <summary>
        /// ContainerBuilder是构建Autofac容器的构建器,通过这个委托可以注册一个服务,该服务可以通过ContainerBuilder注册服务所依赖的组件,
        /// 一般用于注册微服务本身
        /// </summary>
        private readonly List<Action<ContainerBuilder>> _registerServicesDelegates;

        /// <summary>
        /// IConfigurationBuilder是配置构建者，通过IConfigurationBuilder可以构建配置,通过该委托可以指定运行时相关的配置文件
        /// </summary>
        private readonly List<Action<IConfigurationBuilder>> _configureDelegates;

        /// <summary>
        /// IContainer 是Autofac Ioc容器,通过该委托可以为主机注入运行时所依赖的组件
        /// </summary>
        private readonly List<Action<IContainer>> _mapServicesDelegates;

        /// <summary>
        /// ILoggingBuilder是一个抽象的构建者，通过该委托可以配置主机运行时的日志选项
        /// </summary>
        private Action<ILoggingBuilder> _loggingDelegate;

        public ServiceHostBuilder()
        {
            _configureServicesDelegates = new List<Action<IServiceCollection>>();
            _registerServicesDelegates = new List<Action<ContainerBuilder>>();
            _configureDelegates = new List<Action<IConfigurationBuilder>>();
            _mapServicesDelegates = new List<Action<IContainer>>();
        }

        /// <summary>
        /// 构造主机,用于托管应用
        /// </summary>
        /// <returns></returns>
        public IServiceHost Build()
        {
            var services = BuildCommonServices();  //构建微服务依赖的公共组件，并获取ServiceCollection容器
            var config = Configure(); //指定配置文件和获取配置构建者
            if (_loggingDelegate != null) //指定配置配置
                services.AddLogging(_loggingDelegate);
            else
                services.AddLogging();
            services.AddSingleton(typeof(IConfigurationBuilder), config);
            var hostingServices = RegisterServices(); //执行注册服务方法,并获取Autofac容器
            var applicationServices = services.Clone();
            var hostingServiceProvider = services.BuildServiceProvider();
            hostingServices.Populate(services);//将ServiceCollection容器中的对象同步到Autofac容器中
            var hostLifetime = hostingServiceProvider.GetService<IHostLifetime>();
            var host = new ServiceHost(hostingServices, hostingServiceProvider, hostLifetime, _mapServicesDelegates);
            var container = host.Initialize();
            return host;
        }

        /// <summary>
        /// 注入主机运行时所依赖的组件
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public IServiceHostBuilder MapServices(Action<IContainer> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapServicesDelegates.Add(mapper);
            return this;
        }

        /// <summary>
        /// 注册服务，并通过builder指定服务所依赖的组件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public IServiceHostBuilder RegisterServices(Action<ContainerBuilder> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _registerServicesDelegates.Add(builder);
            return this;
        }

        /// <summary>
        /// 注册IServiceCollection 容器所依赖的组件
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        public IServiceHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices == null)
            {
                throw new ArgumentNullException(nameof(configureServices));
            }
            _configureServicesDelegates.Add(configureServices);
            return this;
        }

        /// <summary>
        /// 指定配置文件
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        public IServiceHostBuilder Configure(Action<IConfigurationBuilder> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _configureDelegates.Add(builder);
            return this;
        }

        /// <summary>
        /// 构建公用的组件
        /// </summary>
        /// <returns></returns>
        private IServiceCollection BuildCommonServices()
        {
            var services = new ServiceCollection();
            foreach (var configureServices in _configureServicesDelegates)
            {
                configureServices(services);
            }
            return services;
        }

        /// <summary>
        ///根据制定的配置文件获取配置构建者
        /// </summary>
        /// <returns></returns>
        private IConfigurationBuilder Configure()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);
            foreach (var configure in _configureDelegates)
            {
                configure(config);
            }
            return config;
        }

        /// <summary>
        /// 获取Autofac 容器构建者
        /// </summary>
        /// <returns></returns>
        private ContainerBuilder RegisterServices()
        {
            var hostingServices = new ContainerBuilder();
            foreach (var registerServices in _registerServicesDelegates)
            {
                registerServices(hostingServices);
            }
            return hostingServices;
        }

        /// <summary>
        /// 配置日志
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public IServiceHostBuilder ConfigureLogging(Action<ILoggingBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            _loggingDelegate = configure;
            return this;
        }
    }
}