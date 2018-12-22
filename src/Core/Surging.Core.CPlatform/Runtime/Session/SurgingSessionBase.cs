namespace Surging.Core.CPlatform.Runtime.Session
{
    public abstract class SurgingSessionBase : ISurgingSession
    {
        public abstract string UserId { get; }
        public abstract string UserName { get; }
    }
}