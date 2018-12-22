using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.ServiceHosting.Startup;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.ServiceHosting.Internal.Implementation
{
    /// <summary>
    /// 微服务主机(容器)
    /// </summary>
    public class ServiceHost : IServiceHost
    {
        /// <summary>
        /// Autofac Ioc容器构建者，用于注册微服务运行时组件或创建、更新微服务Ioc容器
        /// </summary>
        private readonly ContainerBuilder _builder;

        private IStartup _startup;

        /// <summary>
        /// Ioc容器，用于管理应用运行时所有依赖的组件
        /// </summary>
        private IContainer _applicationServices;

        /// <summary>
        /// 描述主机容器的生命周期
        /// </summary>
        private readonly IHostLifetime _hostLifetime;

        /// <summary>
        /// 主机服务提供者
        /// </summary>
        private readonly IServiceProvider _hostingServiceProvider;

        /// <summary>
        /// 用于注册组件的一个委托集合，通过这个委托集合，可以向运行时注册所依赖的组件
        /// </summary>
        private readonly List<Action<IContainer>> _mapServicesDelegates;

        /// <summary>
        /// 描述应用的生命周期
        /// </summary>
        private IApplicationLifetime _applicationLifetime;

        /// <summary>
        /// 微服务主机(容器构造器)
        /// </summary>
        /// <param name="builder">Ioc容器构建者</param>
        /// <param name="hostingServiceProvider">主机服务提供者</param>
        /// <param name="hostLifetime">主机的生命周期</param>
        /// <param name="mapServicesDelegate">组件服务注册者委托</param>
        public ServiceHost(ContainerBuilder builder,
            IServiceProvider hostingServiceProvider,
            IHostLifetime hostLifetime,
             List<Action<IContainer>> mapServicesDelegate)
        {
            _builder = builder;
            _hostingServiceProvider = hostingServiceProvider;
            _hostLifetime = hostLifetime;
            _mapServicesDelegates = mapServicesDelegate;
        }

        /// <summary>
        /// 停止服务时，需要释放主机占用的资源
        /// </summary>
        public void Dispose()
        {
            (_hostingServiceProvider as IDisposable)?.Dispose();
        }

        /// <summary>
        /// 启动主机
        /// </summary>
        /// <returns></returns>
        public IDisposable Run()
        {
            RunAsync().GetAwaiter().GetResult();
            return this;
        }

        /// <summary>
        /// 以异步的方式启动主机
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RunAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_applicationServices != null)
                MapperServices(_applicationServices);

            if (_hostLifetime != null)
            {
                _applicationLifetime = _hostingServiceProvider.GetService<IApplicationLifetime>();
                await _hostLifetime.WaitForStartAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _applicationLifetime?.NotifyStarted();
            }
        }

        /// <summary>
        /// 停止主机
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var cts = new CancellationTokenSource(2000))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken))
            {
                var token = linkedCts.Token;
                _applicationLifetime?.StopApplication();
                token.ThrowIfCancellationRequested();
                await _hostLifetime.StopAsync(token);
                _applicationLifetime?.NotifyStopped();
            }
        }

        /// <summary>
        /// 初始化主机/应用所依赖的组件，并获取到Ioc容器
        /// </summary>
        /// <returns></returns>
        public IContainer Initialize()
        {
            if (_applicationServices == null)
            {
                _applicationServices = BuildApplication();
            }
            return _applicationServices;
        }

        /// <summary>
        /// 确保构建了运行主机所依赖的必要的组件
        /// </summary>
        private void EnsureApplicationServices()
        {
            if (_applicationServices == null)
            {
                EnsureStartup();
                _applicationServices = _startup.ConfigureServices(_builder);
            }
        }

        /// <summary>
        /// 确保服务主机配置了启动类(Startup类)
        /// </summary>
        private void EnsureStartup()
        {
            if (_startup != null)
            {
                return;
            }

            _startup = _hostingServiceProvider.GetRequiredService<IStartup>();
        }

        /// <summary>
        /// 构建应用所依赖的必要的组件
        /// </summary>
        /// <returns></returns>
        private IContainer BuildApplication()
        {
            try
            {
                EnsureApplicationServices();
                Action<IContainer> configure = _startup.Configure;
                if (_applicationServices == null)
                    _applicationServices = _builder.Build();
                configure(_applicationServices);
                return _applicationServices;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("应用程序启动异常: " + ex.ToString());
                throw;
            }
        }

        private void MapperServices(IContainer mapper)
        {
            foreach (var mapServices in _mapServicesDelegates)
            {
                mapServices(mapper);
            }
        }
    }
}