using System.Threading;

namespace Surging.Core.ServiceHosting.Internal
{
    /// <summary>
    /// 一个抽象的应用生命周期,应用是指寄宿在Host的dll
    /// </summary>
    public interface IApplicationLifetime
    {
        /// <summary>
        ///  应用启动时状态点
        /// </summary>
        CancellationToken ApplicationStarted { get; }

        /// <summary>
        /// 应用停止时状态点
        /// </summary>
        CancellationToken ApplicationStopping { get; }

        /// <summary>
        /// 应用停止后状态点
        /// </summary>
        CancellationToken ApplicationStopped { get; }

        /// <summary>
        /// 触发停止应用事件
        /// </summary>

        void StopApplication();

        /// <summary>
        /// 触发应用停止后事件
        /// </summary>
        void NotifyStopped();

        /// <summary>
        /// 触发应用启动事件
        /// </summary>
        void NotifyStarted();
    }
}