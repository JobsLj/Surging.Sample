using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Surging.Core.ServiceHosting.Internal
{
    /// <summary>
    /// 抽象的主机构建者
    /// </summary>
    public interface IServiceHostBuilder
    {
        /// <summary>
        /// 构建主机
        /// </summary>
        /// <returns></returns>
        IServiceHost Build();

        /// <summary>
        /// 一般用于注册微服务,通过IocBuidler来注入微服务(应用)运行时所依赖的组件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IServiceHostBuilder RegisterServices(Action<ContainerBuilder> builder);

        /// <summary>
        /// 为主机配置日志
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        IServiceHostBuilder ConfigureLogging(Action<ILoggingBuilder> configure);

        /// <summary>
        /// 注册IServiceCollection 容器所依赖的组件
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        IServiceHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// 指定系统运行时的的配置文件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IServiceHostBuilder Configure(Action<IConfigurationBuilder> builder);

        /// <summary>
        /// 配置主机运行时依赖的组件
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        IServiceHostBuilder MapServices(Action<IContainer> mapper);
    }
}