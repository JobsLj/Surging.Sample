namespace Surging.Core.CPlatform.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// 负载分流策略
    /// </summary>
    public enum AddressSelectorMode
    {
        /// <summary>
        /// 哈希算法
        /// </summary>
        HashAlgorithm,

        /// <summary>
        /// 轮询
        /// </summary>
        Polling,

        /// <summary>
        /// 随机
        /// </summary>
        Random,

        /// <summary>
        /// 压力最小优先
        /// </summary>
        FairPolling,
    }
}