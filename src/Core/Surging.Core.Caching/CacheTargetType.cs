namespace Surging.Core.Caching
{
    /// <summary>
    /// 缓存方式或缓存技术枚举，如：Redis/CouchBase/Memcached/MemoryCache。
    /// </summary>
    public enum CacheTargetType
    {
        Redis,
        CouchBase,
        Memcached,
        MemoryCache,
    }
}