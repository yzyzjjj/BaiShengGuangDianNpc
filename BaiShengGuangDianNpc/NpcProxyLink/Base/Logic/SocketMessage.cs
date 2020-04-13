using System;
using System.Collections.Generic;
using System.Linq;
using ModelBase.Base.Utils;
using ServiceStack;

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
        public string Data => Deserialize();
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

        private string Deserialize()
        {
            try
            {
                var datas = DataStrList.ToArray();
                //包头 F3
                //功能码 02
                //字节数 4bytes
                //唯一识别码   4bytes
                if (datas.Length == 0 || datas[0] != "f3" || datas[1] != "02")
                {
                    return DataStrList.Join(",");
                }

                var trueData = datas.Skip(10);
                if (!trueData.Any())
                {
                    return DataStrList.Join(",");
                }
                //变量数据
                //变量1(4bytes)
                //变量2
                //…
                //变量N
                var vals = new List<int>();
                var i = 0;
                for (i = 0; i < ValNum; i++)
                {
                    var str = trueData.Skip(i * 4).Take(4).Reverse().Join("");
                    var v = Convert.ToInt32(str, 16);
                    vals.Add(v);
                }
                //I口数据    
                //I1~I8(1bytes)
                //I9~I17
                //…
                //IN~I(N + 8)
                trueData = trueData.Skip(i * 4);
                var ins = new List<int>();
                for (i = 0; i < trueData.Count(); i++)
                {
                    if (ins.Count >= InNum)
                    {
                        break;
                    }
                    var str = trueData.Skip(i).Take(1).Join("");
                    var v = Convert.ToInt32(str, 16);
                    for (var j = 0; j < 8; j++)
                    {
                        if (ins.Count >= InNum)
                        {
                            break;
                        }
                        var temp1 = (byte)((v & 0xff) << (7 - j));
                        var temp2 = (byte)((temp1 >> 7) & 0x01);
                        ins.Add(temp2);
                    }
                }
                //O口数据 
                //O1~O8(1bytes)
                //O9~O17
                //…
                //ON~O(N + 8)
                trueData = trueData.Skip(i);
                var outs = new List<int>();
                for (i = 0; i < trueData.Count(); i++)
                {
                    if (outs.Count >= OutNum)
                    {
                        break;
                    }
                    var str = trueData.Skip(i).Take(1).Join("");
                    var v = Convert.ToInt32(str, 16);
                    for (var j = 0; j < 8; j++)
                    {
                        if (outs.Count >= OutNum)
                        {
                            break;
                        }
                        var temp1 = (byte)((v & 0xff) << (7 - j));
                        var temp2 = (byte)((temp1 >> 7) & 0x01);
                        outs.Add(temp2);
                    }
                }
                //CRC校验   2bytes

                //trueData = trueData.Skip(i);
                //var crc = CrcHelper.GetCrc16(datas.Take(datas.Length - 2));

                return new
                {
                    vals,
                    ins,
                    outs
                }.ToJSON();
            }
            catch (Exception e)
            {
                return "解析错误" + DataStrList.Join(",");
            }
        }
    }
}