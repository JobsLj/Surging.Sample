using Microsoft.Extensions.Configuration;
using Surging.Core.CPlatform.Configurations.Remote;
using System.IO;

namespace Surging.Core.EventBusRabbitMQ.Configurations
{
    public class EventBusConfigurationProvider : FileConfigurationProvider
    {
        public EventBusConfigurationProvider(EventBusConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            var parser = new JsonConfigurationParser();
            this.Data = parser.Parse(stream, null);
        }
    }
}