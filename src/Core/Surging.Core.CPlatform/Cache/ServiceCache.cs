using System;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Core.CPlatform.Cache
{
    public class ServiceCache
    {
        /// <summary>
        /// 服务可用地址。
        /// </summary>
        public IEnumerable<CacheEndpoint> CacheEndpoint { get; set; }

        /// <summary>
        /// 服务描述符。
        /// </summary>
        public CacheDescriptor CacheDescriptor { get; set; }

        #region Equality members

        /// <summary>确定指定的对象是否等于当前对象</summary>
        /// <returns>如果指定的对象等于当前对象，则为true；否则，为false。</returns>
        /// <param name="obj">要与当前对象进行比较的对象。 </param>
        public override bool Equals(object obj)
        {
            var model = obj as ServiceCache;
            if (model == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (model.CacheDescriptor != CacheDescriptor)
                return false;

            return model.CacheEndpoint.Count() == CacheEndpoint.Count() && model.CacheEndpoint.All(addressModel => CacheEndpoint.Contains(addressModel));
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(ServiceCache model1, ServiceCache model2)
        {
            return Equals(model1, model2);
        }

        public static bool operator !=(ServiceCache model1, ServiceCache model2)
        {
            return !Equals(model1, model2);
        }

        #endregion Equality members
    }
}