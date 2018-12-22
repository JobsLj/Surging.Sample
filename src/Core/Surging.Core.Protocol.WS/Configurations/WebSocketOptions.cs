namespace Surging.Core.Protocol.WS.Configurations
{
    public class WebSocketOptions
    {
        public int WaitTime { get; set; } = 1;

        public bool KeepClean
        {
            get;
            set;
        } = false;

        public BehaviorOption Behavior { get; set; }
    }
}