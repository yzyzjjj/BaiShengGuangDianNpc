using ModelBase.Base.Utils;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NpcProxyLink.Base.Logic
{
    public class SocketMessage
    {
        public DateTime SendTime { get; set; }
        public DateTime ReceiveTime { get; set; }
        public int DealTime => (int)(ReceiveTime - SendTime).TotalMilliseconds;
        public bool UserSend { get; set; }
        public List<byte> DataList { get; set; } = new List<byte>();
        public IEnumerable<string> DataStrList => DataList.Select(t => Convert.ToString(t, 16).PadLeft(2, '0'));
        public string Data => DataStrList.Join(",");
        public int DeviceId { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public int ScriptId { get; set; }
        public int ValNum { get; set; }
        public int InNum { get; set; }
        public int OutNum { get; set; }

        public bool IsAll()
        {
            if (!DataStrList.Any())
            {
                return false;
            }

            var len = DataStrList.Count();
            var body = DataStrList.Take(len - 2);
            var crc = CrcHelper.GetCrc16(body).ToArray();
            return crc.Length == 2 && crc[1] == DataStrList.ElementAt(len - 1) && crc[0] == DataStrList.ElementAt(len - 2);
        }
    }
}