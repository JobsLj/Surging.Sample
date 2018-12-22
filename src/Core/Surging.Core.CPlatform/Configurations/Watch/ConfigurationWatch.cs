using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Configurations.Watch
{
    public abstract class ConfigurationWatch
    {
        protected ConfigurationWatch()
        {
        }

        public abstract Task Process();
    }
}