using Surging.Core.Consul.Configurations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.Consul.WatcherProvider.Implementation
{
    public class ClientWatchManager : IClientWatchManager
    {
        internal Dictionary<string, HashSet<Watcher>> dataWatches =
            new Dictionary<string, HashSet<Watcher>>();

        private readonly Timer _timer;

        public ClientWatchManager(ConfigInfo config)
        {
            var timeSpan = TimeSpan.FromSeconds(config.WatchInterval);
            _timer = new Timer(async s =>
            {
                await Watching();
            }, null, timeSpan, timeSpan);
        }

        public Dictionary<string, HashSet<Watcher>> DataWatches
        {
            get
            {
                return dataWatches;
            }
            set
            {
                dataWatches = value;
            }
        }

        private HashSet<Watcher> Materialize()
        {
            HashSet<Watcher> result = new HashSet<Watcher>();
            lock (dataWatches)
            {
                foreach (HashSet<Watcher> ws in dataWatches.Values)
                {
                    result.UnionWith(ws);
                }
            }
            return result;
        }

        private async Task Watching()
        {
            var watches = Materialize();
            foreach (var watch in watches)
            {
                await watch.Process();
            }
        }
    }
}