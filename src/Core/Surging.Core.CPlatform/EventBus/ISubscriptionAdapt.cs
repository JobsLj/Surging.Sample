namespace Surging.Core.CPlatform.EventBus
{
    public interface ISubscriptionAdapt
    {
        void SubscribeAt();

        void Unsubscribe();
    }
}