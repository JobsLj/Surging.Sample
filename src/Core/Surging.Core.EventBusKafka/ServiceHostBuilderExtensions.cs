using Autofac;
using Surging.Core.CPlatform.EventBus;
using Surging.Core.ServiceHosting.Internal;

namespace Surging.Core.EventBusKafka
{
    public static class ServiceHostBuilderExtensions
    {
        public static IServiceHostBuilder SubscribeAt(this IServiceHostBuilder hostBuilder)
        {
            return hostBuilder.MapServices(mapper =>
            {
                mapper.Resolve<ISubscriptionAdapt>().SubscribeAt();
            });
        }
    }
}