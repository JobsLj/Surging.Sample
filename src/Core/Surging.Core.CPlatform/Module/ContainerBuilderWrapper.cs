using Autofac;

namespace Surging.Core.CPlatform.Module
{
    public class ContainerBuilderWrapper
    {
        public ContainerBuilder ContainerBuilder { get; private set; }

        public ContainerBuilderWrapper(ContainerBuilder builder)
        {
            ContainerBuilder = builder;
        }

        public IContainer Build()
        {
            return ContainerBuilder.Build();
        }
    }
}