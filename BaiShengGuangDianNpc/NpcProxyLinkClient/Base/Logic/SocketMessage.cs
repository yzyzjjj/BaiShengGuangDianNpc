using ModelBase.Base.Utils;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NpcProxyLinkClient.Base.Logic
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

        public bool IsAll()
        {
            if (!DataStrList.Any())
            {
                return false;
            }

            var body = DataStrList.Take(DataStrList.Count() - 2);
            var crc = CrcHelper.GetCrc16(body).ToArray();
            var len = DataStrList.Count();
            return crc.Length == 2 && crc[1] == DataStrList.ElementAt(len - 1) && crc[0] == DataStrList.ElementAt(len - 2);
        }
    }
}