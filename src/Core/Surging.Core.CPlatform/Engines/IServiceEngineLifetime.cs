using System.Threading;

namespace Surging.Core.CPlatform.Engines
{
    /// <summary>
    /// 服务引擎生命周期
    /// </summary>
    public interface IServiceEngineLifetime
    {
        CancellationToken ServiceEngineStarted { get; }

        CancellationToken ServiceEngineStopping { get; }

        CancellationToken ServiceEngineStopped { get; }

        void StopApplication();

        void NotifyStopped();

        void NotifyStarted();
    }
}