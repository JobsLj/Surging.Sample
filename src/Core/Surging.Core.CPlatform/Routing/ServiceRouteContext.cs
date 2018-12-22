using Surging.Core.CPlatform.Messages;

namespace Surging.Core.CPlatform.Routing
{
    public class ServiceRouteContext
    {
        public ServiceRoute Route { get; set; }

        public RemoteInvokeResultMessage ResultMessage { get; set; }

        public RemoteInvokeMessage InvokeMessage { get; set; }
    }
}