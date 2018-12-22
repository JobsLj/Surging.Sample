using Autofac;
using Surging.Core.AutoMapper;
using Surging.Core.ServiceHosting.Internal;

namespace Surging.Core
{
    public static class ServiceHostBuilderExtensions
    {
        public static IServiceHostBuilder UserAutoMapper(this IServiceHostBuilder hostBuilder)
        {
            return hostBuilder.MapServices(mapper =>
            {
                var autoMapperBootstrap = mapper.Resolve<IAutoMapperBootstrap>();
                autoMapperBootstrap.Initialize();
            });
        }
    }
}