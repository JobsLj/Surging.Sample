using Microsoft.Extensions.Configuration;

namespace Surging.Core.Consul
{
    public class AppConfig
    {
        public static IConfigurationRoot Configuration { get; set; }
    }
}