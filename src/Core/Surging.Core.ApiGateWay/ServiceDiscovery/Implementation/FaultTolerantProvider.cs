using Surging.Core.CPlatform.Support;
using Surging.Core.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.Core.ApiGateWay.ServiceDiscovery.Implementation
{
    public class FaultTolerantProvider : ServiceBase, IFaultTolerantProvider
    {
        public async Task<IEnumerable<ServiceCommandDescriptor>> GetCommandDescriptor(params string[] serviceIds)
        {
            return await GetService<IServiceCommandManager>().GetServiceCommandsAsync(serviceIds);
        }

        public async Task<IEnumerable<ServiceCommandDescriptor>> GetCommandDescriptorByAddress(string address)
        {
            var services = await GetService<IServiceDiscoveryProvider>().GetServiceDescriptorAsync(address);
            return await GetService<IServiceCommandManager>().GetServiceCommandsAsync(services.Select(p => p.Id).ToArray());
        }

        public async Task SetCommandDescriptorByAddress(ServiceCommandDescriptor model)
        {
            await GetService<IServiceCommandManager>().SetServiceCommandsAsync(new ServiceCommandDescriptor[] { model });
        }
    }
}