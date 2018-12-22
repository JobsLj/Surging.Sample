using Surging.Core.CPlatform.Address;

namespace Surging.Core.ApiGateWay.ServiceDiscovery.Implementation
{
    public class ServiceAddressModel
    {
        public AddressModel Address { get; set; }

        public bool IsHealth { get; set; }
    }
}