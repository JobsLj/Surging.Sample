using Surging.Core.CPlatform.Address;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.Core.CPlatform.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    public class HashAlgorithmAdrSelector : AddressSelectorBase
    {
        public HashAlgorithmAdrSelector()
        {
        }

        #region Overrides of AddressSelectorBase

        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        protected override Task<AddressModel> SelectAsync(AddressSelectContext context)
        {
            var address = context.Address.ToArray();
            var index = context.HashCode % address.Length;
            return Task.FromResult(address[index]);
        }

        #endregion Overrides of AddressSelectorBase
    }
}