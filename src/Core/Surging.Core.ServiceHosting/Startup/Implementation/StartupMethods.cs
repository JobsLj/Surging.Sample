using Autofac;
using System;
using System.Diagnostics;

namespace Surging.Core.ServiceHosting.Startup.Implementation
{
    /// <summary>
    /// 用于描述Startup启动类
    /// </summary>
    public class StartupMethods
    {
        public StartupMethods(object instance, Action<IContainer> configure, Func<ContainerBuilder, IContainer> configureServices)
        {
            Debug.Assert(configure != null);
            Debug.Assert(configureServices != null);

            StartupInstance = instance;
            ConfigureDelegate = configure;
            ConfigureServicesDelegate = configureServices;
        }

        /// <summary>
        /// Startup实例对象
        /// </summary>
        public object StartupInstance { get; }

        /// <summary>
        /// Startup ConfigService方法
        /// </summary>
        public Func<ContainerBuilder, IContainer> ConfigureServicesDelegate { get; }

        public Action<IContainer> ConfigureDelegate { get; }
    }
}