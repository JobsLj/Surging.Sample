using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Routing.Implementation
{
    public abstract class AbstractServiceRouteFactory : IServiceRouteFactory
    {
        protected readonly ISerializer<string> _serializer;
        protected readonly ConcurrentDictionary<string, AddressModel> _addressModel = new ConcurrentDictionary<string, AddressModel>();

        protected AbstractServiceRouteFactory(ISerializer<string> serializer)
        {
            _serializer = serializer;
        }

        #region Implementation of IServiceRouteFactory

        /// <summary>
        /// 根据服务路由描述符创建服务路由。
        /// </summary>
        /// <param name="descriptors">服务路由描述符。</param>
        /// <returns>服务路由集合。</returns>
        public virtual Task<IEnumerable<ServiceRoute>> CreateServiceRoutesAsync(IEnumerable<ServiceRouteDescriptor> descriptors)
        {
            if (descriptors == null)
                throw new ArgumentNullException(nameof(descriptors));

            descriptors = descriptors.ToArray();
            var routes = new List<ServiceRoute>(descriptors.Count());

            routes.AddRange(descriptors.Select(descriptor => new ServiceRoute
            {
                Address = CreateAddress(descriptor.AddressDescriptors),
                ServiceDescriptor = descriptor.ServiceDescriptor
            }));

            return Task.FromResult(routes.AsEnumerable());
        }

        #endregion Implementation of IServiceRouteFactory

        protected abstract IEnumerable<AddressModel> CreateAddress(IEnumerable<ServiceAddressDescriptor> addressDescriptors);
    }
}