using Autofac;
using System;

namespace Surging.Core.ServiceHosting.Internal
{
    /// <summary>
    /// 一个抽象的主机容器
    /// </summary>
    public interface IServiceHost : IDisposable
    {
        /// <summary>
        /// 启用主机
        /// </summary>
        /// <returns></returns>
        IDisposable Run();

        /// <summary>
        /// 初始化主机,构建主机运行时组件的Ioc容器
        /// </summary>
        /// <returns></returns>
        IContainer Initialize();
    }
}