namespace Surging.Core.Protocol.WS.Extensions
{
    public static class WebSocketSessionManagerExtension
    {
        public static void SendTo<T>(this WebSocketSessionManager webSocketSessionManager, T message, string sessionId)
             where T : class
        {
            if (message == null)
            {
                throw new ArgumentNullException("消息不允许为空");
            }
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentNullException("SessionId不允许为空");
            }
            var messageJsonString = JsonConvert.SerializeObject(message);
            var sendData = Encoding.ASCII.GetBytes(messageJsonString);
            webSocketSessionManager.SendTo(sendData, sessionId);
        }
    }
}