using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Surging.Core.CPlatform.Utilities;
using System.IO;

namespace Surging.Core.Schedule.Configurations
{
    public static class QuartzConfigurationExtensions
    {
        public static IConfigurationBuilder AddQuartzFile(this IConfigurationBuilder builder, string path)
        {
            return AddQuartzFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddQuartzFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddQuartzFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddQuartzFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddQuartzFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddQuartzFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
        {
            Check.NotNull(builder, "builder");
            Check.CheckCondition(() => string.IsNullOrEmpty(path), "path");
            path = CPlatform.Utilities.EnvironmentHelper.GetEnvironmentVariable(path);
            if (provider == null && Path.IsPathRooted(path))
            {
                provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
                path = Path.GetFileName(path);
            }
            var source = new QuartzConfigurationSource
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            builder.Add(source);
            AppConfig.Path = path;
            AppConfig.Configuration = builder.Build();
            return builder;
        }
    }
}