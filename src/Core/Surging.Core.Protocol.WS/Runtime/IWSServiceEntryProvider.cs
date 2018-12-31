using System.Collections.Generic;

namespace Surging.Core.Protocol.WS.Runtime
{
    public interface IWSServiceEntryProvider
    {
        IEnumerable<WSServiceEntry> GetEntries();
    }
}