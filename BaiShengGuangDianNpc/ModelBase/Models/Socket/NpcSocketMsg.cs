using ModelBase.Base.EnumConfig;
using System;

namespace ModelBase.Models.Socket
{
    public class NpcSocketMsg
    {
        public string Guid = string.Empty;
        public NpcSocketMsgType MsgType;
        public string Body = string.Empty;
    }
}
