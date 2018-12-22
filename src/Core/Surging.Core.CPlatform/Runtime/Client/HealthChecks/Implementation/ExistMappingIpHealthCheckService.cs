using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Runtime.Client.HealthChecks.Implementation
{
    public class ExistMappingIpHealthCheckService : HealthCheckServiceBase, IDisposable
    {
        protected readonly ConcurrentDictionary<ValueTuple<string, int>, ValueTuple<string, int>> _innerDictionary = new ConcurrentDictionary<ValueTuple<string, int>, ValueTuple<string, int>>();

        public ExistMappingIpHealthCheckService(IServiceRouteManager serviceRouteManager) : base(serviceRouteManager)
        {
        }

        #region Implementation of IHealthCheckService

        /// <summary>
        /// 监控一个地址。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>一个任务。</returns>
        public override void Monitor(AddressModel address)
        {
            var ipAddress = address as MappingAddressModel;
            _dictionary.GetOrAdd(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), k => new MonitorEntry(address));
            _innerDictionary.GetOrAdd(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), new ValueTuple<string, int>(ipAddress.InnerIp, ipAddress.InnerPort));
        }

        /// <summary>
        /// 判断一个地址是否健康。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>健康返回true，否则返回false。</returns>
        public override ValueTask<bool> IsHealth(AddressModel address)
        {
            var ipAddress = address as MappingAddressModel;
            MonitorEntry entry;
            return !_dictionary.TryGetValue(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), out entry) ? new ValueTask<bool>(Check(address, _timeout)) : new ValueTask<bool>(entry.Health);
        }

        /// <summary>
        /// 标记一个地址为失败的。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>一个任务。</returns>
        public override Task MarkFailure(AddressModel address)
        {
            return Task.Run(() =>
            {
                var ipAddress = address as MappingAddressModel;
                var entry = _dictionary.GetOrAdd(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), k => new MonitorEntry(address, false));
                entry.Health = false;
            });
        }

        #endregion Implementation of IHealthCheckService

        #region protected Method

        protected override void RemoveUnhealthyAddress(IEnumerable<HealthCheckServiceBase.MonitorEntry> monitorEntry)
        {
            if (monitorEntry.Any())
            {
                var addresses = monitorEntry.Select(p =>
                {
                    var ipEndPoint = p.EndPoint as IPEndPoint;
                    _innerDictionary.TryGetValue((ipEndPoint.Address.ToString(), ipEndPoint.Port), out (string, int) innerPoint);
                    return new MappingAddressModel(innerPoint.Item1, innerPoint.Item2, ipEndPoint.Address.ToString(), ipEndPoint.Port);
                }).ToList();

                _serviceRouteManager.RemveAddressAsync(addresses).Wait();
                addresses.ForEach(p =>
                {
                    var ipAddress = p as MappingAddressModel;
                    _dictionary.TryRemove(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), out MonitorEntry value);
                    _innerDictionary.TryRemove(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), out (string, int) innerPoint);
                });
            }
        }

        protected override void Remove(IEnumerable<AddressModel> addressModels)
        {
            foreach (var addressModel in addressModels)
            {
                MonitorEntry value;
                var ipAddress = addressModel as MappingAddressModel;
                _dictionary.TryRemove(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), out value);
                _innerDictionary.TryRemove(new ValueTuple<string, int>(ipAddress.MappingIp, ipAddress.MappingPort), out ValueTuple<string, int> innerEndpint);
            }
        }

        protected override ICollection<(string, int)> GetEndpointInfo(IEnumerable<AddressModel> addressModels)
        {
            var keys = new List<(string, int)>();
            foreach (var address in addressModels)
            {
                var domainAddress = address as MappingAddressModel;
                keys.Add(new ValueTuple<string, int>(domainAddress.MappingIp, domainAddress.MappingPort));
                _innerDictionary.GetOrAdd(new ValueTuple<string, int>(domainAddress.MappingIp, domainAddress.MappingPort),
                    new ValueTuple<string, int>(domainAddress.InnerIp, domainAddress.InnerPort));
            }
            return keys;
        }

        #endregion protected Method
    }
}