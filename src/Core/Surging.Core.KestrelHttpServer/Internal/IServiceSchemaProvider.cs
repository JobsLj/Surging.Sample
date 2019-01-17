using System.Collections.Generic;

namespace Surging.Core.KestrelHttpServer.Internal
{
    public interface IServiceSchemaProvider
    {
        IEnumerable<string> GetSchemaFilesPath(string annotationXmlDir);
    }
}