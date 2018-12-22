using System.Threading;
using System.Threading.Tasks;

namespace Surging.Core.ServiceHosting.Internal
{
    /// <summary>
    /// 抽象的主机生命周期
    /// </summary>
    public interface IHostLifetime
    {
        /// <summary>
        /// 等待主机开始运行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WaitForStartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 停止主机服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StopAsync(CancellationToken cancellationToken);
    }
}