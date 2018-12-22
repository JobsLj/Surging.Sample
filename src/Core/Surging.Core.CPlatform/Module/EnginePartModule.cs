namespace Surging.Core.CPlatform.Module
{
    public class EnginePartModule : AbstractModule
    {
        public override void Initialize(CPlatformContainer serviceProvider)
        {
            base.Initialize(serviceProvider);
        }

        protected virtual void RegisterServiceBuilder(IServiceBuilder builder)
        {
        }

        internal override void RegisterComponents(ContainerBuilderWrapper builder)
        {
            base.RegisterComponents(builder);
        }
    }
}