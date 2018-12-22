using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Serialization;
using System.Collections.Generic;

namespace Surging.Core.CPlatform.Routing.Implementation
{
    /// <summary>
    /// 一个默认的服务路由工厂实现。
    /// </summary>
    public class DefaultServiceRouteFactory : AbstractServiceRouteFactory
    {
        public DefaultServiceRouteFactory(ISerializer<string> serializer) : base(serializer)
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
                    try
                    {
                        address = (AddressModel)_serializer.Deserialize(descriptor.Value, typeof(IpAddressModel));
                        _addressModel.TryAdd(descriptor.Value, address);
                    }
                    catch
                    {
                        var mappingAddress = (MappingAddressModel)_serializer.Deserialize(descriptor.Value, typeof(MappingAddressModel));
                        _addressModel.TryAdd(descriptor.Value, new IpAddressModel()
                        {
                            Ip = mappingAddress.MappingIp,
                            Port = mappingAddress.MappingPort
                        });
                    }
                }
                yield return address;
            }
        }
    }
}