using Surging.Core.Caching;
using System;

namespace Surging.Core.System.Intercept
{
    /// <summary>
    /// 设置判断缓存拦截方法的特性类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class InterceptMethodAttribute : Attribute
    {
        #region 字段

        private int _time = 60;
        private CacheTargetType _mode = CacheTargetType.MemoryCache;
        private CachingMethod _method;
        private string[] _correspondingKeys;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 初始化一个新的<c>InterceptMethodAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        public InterceptMethodAttribute(CachingMethod method)
        {
            this._method = method;
        }

        /// <summary>
        /// 初始化一个新的<c>InterceptMethodAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        /// <param name="correspondingMethodNames">与当前缓存方式相关的方法名称。注：此参数仅在缓存方式为Remove时起作用。</param>
        public InterceptMethodAttribute(CachingMethod method, params string[] correspondingMethodNames)
            : this(method)
        {
            this._correspondingKeys = correspondingMethodNames;
        }

        #endregion 构造函数

        #region 公共属性

        /// <summary>
        /// 有效时间
        /// </summary>
        public int Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// 缓存方式或采用的技术（枚举类型）
        /// </summary>
        public CacheTargetType Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        ///// <summary>
        ///// 缓存部件类型（枚举类型），如DLL/接口
        ///// </summary>
        public SectionType CacheSectionType
        {
            get;
            set;
        }

        public string Key { get; set; }

        /// <summary>
        /// 获取或设置缓存方式。
        /// </summary>
        public CachingMethod Method
        {
            get
            {
                return _method;
            }
            set { _method = value; }
        }

        /// <summary>
        /// 获取或设置一个<see cref="Boolean"/>值，该值表示当缓存方式为Put时，是否强制将值写入缓存中。
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// 获取或设置与当前缓存方式相关的方法名称。注：此参数仅在缓存方式为Remove时起作用。
        /// </summary>
        public string[] CorrespondingKeys
        {
            get
            {
                return _correspondingKeys;
            }
            set
            {
                _correspondingKeys = value;
            }
        }

        #endregion 公共属性
    }
}