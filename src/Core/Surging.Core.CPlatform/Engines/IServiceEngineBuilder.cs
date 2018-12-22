using Autofac;

namespace Surging.Core.CPlatform.Engines
{
    /// <summary>
    /// 服务引擎构造者
    /// </summary>
    public interface IServiceEngineBuilder
    {
        void Build(ContainerBuilder serviceContainer);
    }
}