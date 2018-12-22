using System.Collections.Generic;

namespace Surging.Core.CPlatform.Cache
{
    public interface ICacheNodeProvider
    {
        IEnumerable<ServiceCache> GetServiceCaches();
    }
}