using ModelBase.Base.Utils;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using GateProxyLink.Base.Control;

namespace GateProxyLink.Base.Control
{
    /// <summary>
    /// 设置变量
    /// </summary>
    public class SetValMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.SetVal;

        public override int FunctionCode => 5;

        public Dictionary<int, int> Vals = new Dictionary<int, int>();
        ///<summary>
        /// 设置变量报文
        ///</summary>
        ///<returns></returns>
        public override string Serialize()
        {
            ValNum = Vals.Count;
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
            };
            var cData1 = new List<string>();
            var tData1 = new List<string>();
            var cData2 = new List<string>();
            var tData2 = new List<string>();
            foreach (var valuePair in Vals)
            {
                tData1.Clear();
                var key = valuePair.Key;
                for (var i = 0; i < 2; i++)
                {
                    tData1.Add(Convert.ToString((byte)(key >> i * 8 & 0xff), 16));
                }
                cData1.AddRange(tData1);

                tData2.Clear();
                var val = valuePair.Value;
                for (var i = 0; i < 4; i++)
                {
                    tData2.Add(Convert.ToString((byte)(val >> i * 8 & 0xff), 16));
                }
                cData2.AddRange(tData2);
            }
            data.AddRange(cData1);
            data.AddRange(cData2);

            var crc = CrcHelper.GetCrc16(data);
            data.AddRange(crc);
            return data.Join(",");
        }

        /// <summary>
        /// 0 成功 1 失败
        /// </summary>
        /// <returns></returns>
        public override dynamic Deserialize(string response)
        {
            var datas = response.Split(",");
            //包头 F3
            //功能码 05
            if (datas.Length == 0 || datas[0] != "f3" || datas[1] != "05")
            {
                return null;
            }

            //CRC校验   2bytes
            var bCrc = datas.Skip(3);
            var crc = CrcHelper.GetCrc16(datas.Take(datas.Length - 2));
            return datas[2] == "00" ? 0 : 1;
        }
    }
}
