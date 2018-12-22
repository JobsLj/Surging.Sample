using Surging.Core.CPlatform.Address;
using Surging.Core.CPlatform.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Runtime.Client.HealthChecks.Implementation
{
    public abstract class HealthCheckServiceBase : IHealthCheckService, IDisposable
    {
        protected readonly ConcurrentDictionary<ValueTuple<string, int>, MonitorEntry> _dictionary = new ConcurrentDictionary<ValueTuple<string, int>, MonitorEntry>();
        protected readonly IServiceRouteManager _serviceRouteManager;
        protected readonly int _timeout = 30000;
        protected readonly Timer _timer;

        public HealthCheckServiceBase(IServiceRouteManager serviceRouteManager)
        {
            var timeSpan = TimeSpan.FromSeconds(10);

            _serviceRouteManager = serviceRouteManager;
            _timer = new Timer(async s =>
            {
                await Check(_dictionary.ToArray().Select(i => i.Value), _timeout);
                RemoveUnhealthyAddress(_dictionary.ToArray().Select(i => i.Value).Where(m => m.UnhealthyTimes >= 6));
            }, null, timeSpan, timeSpan);

            //去除监控。
            serviceRouteManager.Removed += (s, e) =>
            {
                Remove(e.Route.Address);
            };

            serviceRouteManager.Created += async (s, e) =>
            {
                var keys = GetEndpointInfo(e.Route.Address);
                await Check(_dictionary.Where(i => keys.Contains(i.Key)).Select(i => i.Value), _timeout);
            };
            //重新监控。
            serviceRouteManager.Changed += async (s, e) =>
            {
                var keys = GetEndpointInfo(e.Route.Address);
                await Check(_dictionary.Where(i => keys.Contains(i.Key)).Select(i => i.Value), _timeout);
            };
        }

        #region Implementation of IHealthCheckService

        /// <summary>
        /// 监控一个地址。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>一个任务。</returns>
        public abstract void Monitor(AddressModel address);

        /// <summary>
        /// 判断一个地址是否健康。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>健康返回true，否则返回false。</returns>
        public abstract ValueTask<bool> IsHealth(AddressModel address);

        /// <summary>
        /// 标记一个地址为失败的。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>一个任务。</returns>
        public abstract Task MarkFailure(AddressModel address);

        #endregion Implementation of IHealthCheckService

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _timer.Dispose();
        }

        #endregion Implementation of IDisposable

        #region Private Method

        protected abstract void Remove(IEnumerable<AddressModel> addressModels);

        protected abstract ICollection<ValueTuple<string, int>> GetEndpointInfo(IEnumerable<AddressModel> addressModels);

        protected abstract void RemoveUnhealthyAddress(IEnumerable<MonitorEntry> monitorEntry);

        protected static async Task<bool> Check(AddressModel address, int timeout)
        {
            bool isHealth = false;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { SendTimeout = timeout })
            {
                try
                {
                    await socket.ConnectAsync(address.CreateEndPoint());
                    isHealth = true;
                }
                catch (Exception ex)
                {
                }
                return isHealth;
            }
        }

        protected static async Task Check(IEnumerable<MonitorEntry> entrys, int timeout)
        {
            foreach (var entry in entrys)
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { SendTimeout = timeout })
                {
                    try
                    {
                        await socket.ConnectAsync(entry.EndPoint);
                        entry.UnhealthyTimes = 0;
                        entry.Health = true;
                    }
                    catch
                    {
                        entry.UnhealthyTimes++;
                        entry.Health = false;
                    }
                }
            }
        }

        #endregion Private Method

        #region Help Class

        protected class MonitorEntry
        {
            public MonitorEntry(AddressModel addressModel, bool health = true)
            {
                EndPoint = addressModel.CreateEndPoint();

                Health = health;
            }

            public int UnhealthyTimes { get; set; }

            public EndPoint EndPoint { get; set; }
            public bool Health { get; set; }
        }

        #endregion Help Class
    }
}