using System;

namespace Surging.Core.Caching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IdentifyCacheAttribute : Attribute
    {
        public IdentifyCacheAttribute(CacheTargetType name)
        {
            this.Name = name;
        }

        public CacheTargetType Name { get; set; }
    }
}