using System;

namespace Surging.Core.EventBusKafka
{
    public interface IKafkaPersisterConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        Object CreateConnect();
    }
}