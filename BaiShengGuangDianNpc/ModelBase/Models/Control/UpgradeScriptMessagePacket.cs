using ModelBase.Base.Utils;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelBase.Models.Control
{
    /// <summary>
    /// 开启升级脚本模式
    /// </summary>
    public class UpgradeScriptMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.UpgradeScript;
        public override int FunctionCode => 10;
        public override int SubFunctionCode => 0;
        public int OperateCode => 0;
        public int DataLength { get; set; }
        public int PwdLength { get; set; }

        public UpgradeScriptMessagePacket(int dataLength, int pwdLength = 0)
        {
            Header = "f1";
            DataLength = dataLength;
            PwdLength = pwdLength;
        }
        ///<summary>
        /// 报文
        ///</summary>
        ///<returns></returns>
        public override string Serialize()
        {
            //0xF1 10 00 00 01,41,00,00,脚本长度4字节小端，密码长度4字节小端 CRC
            // 包头 F1
            // 功能码	10
            // 子功能码	00
            // 操作码	00
            // 脚本长度4字节小端	01,41
            // 密码长度4字节小端	00,00
            // CRC校验	2bytes
            var data = new List<string>
            {
                Header,
                FunctionCode.ToString(),
                Convert.ToString(SubFunctionCode, 16),
                Convert.ToString(OperateCode, 16),
               "01",
               "41",
               "00",
               "00"
            };
            var cData = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                cData.Add(Convert.ToString((byte)(DataLength >> i * 8 & 0xff), 16));
            }
            data.AddRange(cData);
            cData.Clear();
            for (var i = 0; i < 4; i++)
            {
                cData.Add(Convert.ToString((byte)(PwdLength >> i * 8 & 0xff), 16));
            }
            data.AddRange(cData);

            var crc = CrcHelper.GetCrc16(data);
            data.AddRange(crc);
            return data.Join(",");
        }

        /// <summary>
        /// 会接收到回复，但没有0xF1 10 00 00这个头
        /// </summary>
        /// <returns></returns>
        public override dynamic Deserialize(string response)
        {
            //成功	01,41,00,00,……	
            //错误	01,C1,00,00,……
            var datas = response.Split(",");
            if (datas.Length <= 2 || datas[0] != "01")
            {
                return 1;
            }

            return datas[1] == "41" ? 0 : 1;
        }
        /// <summary>
        /// 会接收到回复，但没有0xF1 10 00 00这个头
        /// </summary>
        /// <returns></returns>
        public int DeserializeData(string response)
        {
            //发送脚本文件，“脚本数据 crc”，结束会接收到“0x01,0xC1,0x00,0x00,状态码1字节”						
            // 0   成功
            // 1   长度不够
            // 2   crc不通过
            var datas = response.Split(",");
            if (datas.Length == 0 || datas[0] != "01")
            {
                return -1;
            }

            //CRC校验   2bytes
            return int.Parse(datas[5]);
        }
    }
}
