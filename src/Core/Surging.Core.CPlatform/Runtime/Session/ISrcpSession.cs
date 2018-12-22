namespace Surging.Core.CPlatform.Runtime.Session
{
    public interface ISurgingSession
    {
        string UserId { get; }

        string UserName { get; }
    }
}