using System;
using System.Collections.Generic;
using System.Linq;
using GateProxyLink.Base.Control;
using ModelBase.Base.Utils;
using ServiceStack;

namespace GateProxyLink.Base.Control
{
    /// <summary>
    /// 重启
    /// </summary>
    public class ReStartMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.Restart;
        public override int FunctionCode => 3;
        public override int SubFunctionCode => 0;

        ///<summary>
        /// 重启报文
        ///</summary>
        ///<returns></returns>
        public override string Serialize()
        {
            // 包头 F3
            // 功能码	03
            // 子功能码	00
            // CRC校验	2bytes
            var data = new List<string>
            {
                Header,
                Convert.ToString(FunctionCode, 16),
                Convert.ToString(SubFunctionCode, 16),
            };

            var crc = CrcHelper.GetCrc16(data);
            data.AddRange(crc);
            return data.Join(",");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override dynamic Deserialize(string response)
        {
            //包头 F3
            //功能码	03
            //子功能码	00
            //CRC校验	2bytes
            //response = "f3,3,0,0,crc";
            var datas = response.Split(",");
            if (datas.Length == 0 || datas[0] != "f3" || datas[1] != "3")
            {
                return null;
            }

            return datas[3] == "0" ? 0 : 1;
        }
    }
}
