using Microsoft.Extensions.Configuration;

namespace Surging.Core.Schedule.Configurations
{
    public class QuartzConfigurationSource : FileConfigurationSource
    {
        public string ConfigurationKeyPrefix { get; set; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new QuartzConfigurationProvider(this);
        }
    }
}