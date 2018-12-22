using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Serialization;
using System.Collections.Generic;

namespace Surging.Core.CPlatform.Routing.Implementation
{
    public class ExistMappingIpServiceRouteFactory : AbstractServiceRouteFactory
    {
        public ExistMappingIpServiceRouteFactory(ISerializer<string> serializer) : base(serializer)
        {
        }

        protected override IEnumerable<AddressModel> CreateAddress(IEnumerable<ServiceAddressDescriptor> descriptors)
        {
            if (descriptors == null)
                yield break;

            foreach (var descriptor in descriptors)
            {
                _addressModel.TryGetValue(descriptor.Value, out AddressModel address);
                if (address == null)
                {
                    address = (AddressModel)_serializer.Deserialize(descriptor.Value, typeof(MappingAddressModel));
                    _addressModel.TryAdd(descriptor.Value, address);
                }
                yield return address;
            }
        }
    }
}