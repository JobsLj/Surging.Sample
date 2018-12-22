using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Serialization;
using System.Collections.Generic;

namespace Surging.Core.CPlatform.Runtime.Client.Implementation
{
    internal class ExistMappingIpServiceSubscriberFactory : AbstractServiceSubscriberFactory
    {
        public ExistMappingIpServiceSubscriberFactory(ISerializer<string> serializer) : base(serializer)
        {
        }

        protected override IEnumerable<AddressModel> CreateAddress(IEnumerable<ServiceAddressDescriptor> descriptors)
        {
            if (descriptors == null)
                yield break;

            foreach (var descriptor in descriptors)
            {
                yield return (AddressModel)_serializer.Deserialize(descriptor.Value, typeof(MappingAddressModel));
            }
        }
    }
}