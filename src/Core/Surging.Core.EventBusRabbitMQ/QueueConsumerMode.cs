namespace Surging.Core.EventBusRabbitMQ
{
    public enum QueueConsumerMode
    {
        Normal = 0,
        Retry,
        Fail,
    }
}