using ModelBase.Base.Utils;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelBase.Models.Control
{
    public class DeviceInfoMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.DeviceInfo;

        public override int FunctionCode => 2;

        public DeviceInfoMessagePacket(int valNum, int inNum, int outNum)
        {
            ValNum = valNum;
            InNum = inNum;
            OutNum = outNum;
        }

        ///<summary>
        /// 查询报文
        ///</summary>
        ///<returns></returns>
        public override string Serialize()
        {
            //包头 F3
            //功能码 02
            //变量数 2bytes
            //I口数 2bytes
            //O口数 2bytes
            //CRC校验   2bytes
            var data = new List<string>
            {
                Header,
                Convert.ToString(FunctionCode, 16),
                Convert.ToString((byte)(ValNum & 0xff), 16),
                Convert.ToString((byte)((ValNum >> 8) & 0xff), 16),
                Convert.ToString((byte)(InNum & 0xff), 16),
                Convert.ToString((byte)((InNum >> 8) & 0xff), 16),
                Convert.ToString((byte)(OutNum & 0xff), 16),
                Convert.ToString((byte)((OutNum >> 8) & 0xff), 16)
            };

            var crc = CrcHelper.GetCrc16(data);
            data.AddRange(crc);
            return data.Join(",");
        }

        /// <summary>
        /// 0 解锁 1 锁定
        /// </summary>
        /// <returns></returns>
        public override dynamic Deserialize(string response)
        {
            var datas = response.Split(",");
            //包头 F3
            //功能码 02
            //字节数 4bytes
            //唯一识别码   4bytes
            if (datas.Length == 0 || datas[0] != "f3" || datas[1] != "02")
            {
                return null;
            }

            var trueData = datas.Skip(10);
            if (!trueData.Any())
            {
                return null;
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
                var v = 0;
                if (i * 4 + 4 <= trueData.Count())
                {
                    var str = trueData.Skip(i * 4).Take(4).Reverse().Join("");
                    v = Convert.ToInt32(str, 16);
                }
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
            };
        }
    }
}
