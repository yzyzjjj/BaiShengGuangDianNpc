using System;
using ModelBase.Models.Device;

namespace NpcProxyLink.Base.Logic
{
    public class Client
    {

        /// <summary>
        /// 客户端信息
        /// </summary>
        public DeviceInfo DeviceInfo => Socket.DeviceInfo;

        public SocketInfo Socket;
        public DateTime NextSendTime;

        public void Init(DeviceInfo deviceInfo)
        {
            Socket = new SocketInfo(deviceInfo);
        }

        public void Dispose()
        {
            Socket.Disconnect();
        }
    }
}
