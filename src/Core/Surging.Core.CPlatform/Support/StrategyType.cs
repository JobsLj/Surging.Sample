namespace Surging.Core.CPlatform.Support
{
    /// <summary>
    /// 容错策略
    /// </summary>
    public enum StrategyType
    {
        /// <summary>
        /// 失败切换远程服务机制
        /// </summary>
        Failover = 0,

        /// <summary>
        /// 失败执行注入脚本
        /// </summary>
        Injection = 1,

        /// <summary>
        /// 失败执行指定回调方法
        /// </summary>
        FallBack = 2,
    }
}