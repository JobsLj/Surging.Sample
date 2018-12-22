using AutoMapper;
using AutoMapper.Attributes;
using System.Linq;

namespace Surging.Core.AutoMapper
{
    public class AutoMapperBootstrap : IAutoMapperBootstrap
    {
        public void Initialize()
        {
            Mapper.Initialize(config =>
            {
                if (AppConfig.Assemblies.Any())
                {
                    foreach (var assembly in AppConfig.Assemblies)
                    {
                        assembly.MapTypes(config);
                    }
                }
                if (AppConfig.Profiles.Any())
                {
                    foreach (var profile in AppConfig.Profiles)
                    {
                        config.AddProfile(profile);
                    }
                }
            });
        }
    }
}