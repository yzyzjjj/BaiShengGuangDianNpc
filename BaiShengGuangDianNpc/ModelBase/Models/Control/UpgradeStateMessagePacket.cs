using System;
using System.Collections.Generic;
using ModelBase.Base.Utils;
using ServiceStack;

namespace ModelBase.Models.Control
{
    /// <summary>
    /// 升级状态查询
    /// </summary>
    public class UpgradeStateMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.UpgradeState;
        public override int FunctionCode => 4;
        public override int SubFunctionCode => 0;
        public int OperateCode => 1;

        ///<summary>
        /// 解锁报文
        ///</summary>
        ///<returns></returns>
        public override string Serialize()
        {
            // 包头 F3
            // 功能码	04
            // 子功能码	00
            // 操作码	01
            // CRC校验	2bytes
            var data = new List<string>
            {
                Header,
                Convert.ToString(FunctionCode, 16),
                Convert.ToString(SubFunctionCode, 16),
                Convert.ToString(OperateCode, 16)
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
            //包头 F3
            //功能码	4
            //子功能码	0
            //状态码	0 引导层 01 在固件层
            //CRC校验	2bytes
            //response = "f3,4,0,1,b3,61";
            var datas = response.Split(",");
            if (datas.Length == 0 || datas[0] != "f3" || datas[1] != "03")
            {
                return null;
            }

            return datas[3] == "00" ? 0 : 1;
        }
    }
}
