using System;
using ModelBase.Models.Device;

namespace NpcProxyLink.Base.Logic
{
    public class Client
    {

        /// <summary>
        /// 客户端信息
        /// </summary>
        public DeviceInfo DeviceInfo;

        public SocketInfo Socket;
        public DateTime LastSendTime;

        public void Init()
        {
            Socket = new SocketInfo(DeviceInfo);
        }

        public void Dispose()
        {
            Socket.Disconnect();
        }
    }
}
