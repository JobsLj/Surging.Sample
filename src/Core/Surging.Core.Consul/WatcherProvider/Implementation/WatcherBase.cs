using System.Threading.Tasks;

namespace Surging.Core.Consul.WatcherProvider
{
    public abstract class WatcherBase : Watcher
    {
        protected WatcherBase()
        {
        }

        public override async Task Process()
        {
            await ProcessImpl();
        }

        protected abstract Task ProcessImpl();
    }
}