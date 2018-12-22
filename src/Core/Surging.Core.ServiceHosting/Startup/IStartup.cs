using Autofac;

namespace Surging.Core.ServiceHosting.Startup
{
    /// <summary>
    /// 启动类接口,用于配置服务和应用的请求管道
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// 用于配置服务组件
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IContainer ConfigureServices(ContainerBuilder services);

        void Configure(IContainer app);
    }
}