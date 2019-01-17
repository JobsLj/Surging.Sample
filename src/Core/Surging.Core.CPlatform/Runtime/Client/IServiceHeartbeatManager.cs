
namespace Surging.Core.CPlatform.Runtime.Client
{
    public interface IServiceHeartbeatManager
    {
        void AddWhitelist(string serviceId);

        bool ExistsWhitelist(string serviceId);
    }
}
