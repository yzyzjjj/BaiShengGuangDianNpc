using System.Collections.Generic;
using System.Linq;

namespace NpcProxyLink.Base.Logic
{
    public enum SocketMessageEnum
    {
        Default,
        Heart,
        Monitoring,
        Query
    }

    public class SocketMsgManager
    {
        private Dictionary<SocketMessageEnum, List<SocketMessage>> SocketMessages = new Dictionary<SocketMessageEnum, List<SocketMessage>>();

        public SocketMessage GetMsg(SocketMessageEnum socketMessageEnum)
        {
            if (!SocketMessages.ContainsKey(socketMessageEnum))
            {
                SocketMessages.Add(socketMessageEnum, new List<SocketMessage>());
            }

            return SocketMessages[socketMessageEnum].FirstOrDefault();
        }
        public void AddMsg(SocketMessageEnum socketMessageEnum, SocketMessage socketMessage)
        {
            if (!SocketMessages.ContainsKey(socketMessageEnum))
            {
                SocketMessages.Add(socketMessageEnum, new List<SocketMessage>());
            }

            SocketMessages[socketMessageEnum].Add(socketMessage);
        }
        public void RemoveMsg(SocketMessageEnum socketMessageEnum)
        {
            if (!SocketMessages.ContainsKey(socketMessageEnum))
            {
                SocketMessages.Add(socketMessageEnum, new List<SocketMessage>());
            }
            if (SocketMessages[socketMessageEnum].Count > 0)
            {
                SocketMessages[socketMessageEnum].RemoveAt(SocketMessages[socketMessageEnum].Count);
            }
        }
    }
}
