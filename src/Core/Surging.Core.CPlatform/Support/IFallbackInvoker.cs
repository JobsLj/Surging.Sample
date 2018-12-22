using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Support
{
    public interface IFallbackInvoker
    {
        Task Invoke(IDictionary<string, object> parameters, string serviceId, string _serviceKey);

        Task<T> Invoke<T>(IDictionary<string, object> parameters, string serviceId, string _serviceKey);
    }
}