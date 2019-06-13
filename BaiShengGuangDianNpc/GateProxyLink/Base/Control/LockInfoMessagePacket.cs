﻿using ModelBase.Base.Utils;
using ServiceStack;
using System;
using System.Collections.Generic;
using GateProxyLink.Base.Control;

namespace GateProxyLink.Base.Control
{
    ///<summary>
    /// 查询锁定状态
    ///</summary>
    public class LockInfoMessagePacket : MessagePacket
    {
        public override ControlEnum ControlEnum => ControlEnum.LockInfo;
        public override int FunctionCode => 3;
        public override int SubFunctionCode => 1;
        public int OperateCode => 2;

        ///<summary>
        /// 查询报文
        ///</summary>
        ///<returns></returns>
        public override string Serialize()
        {
            // 包头 F3
            // 功能码	03
            // 子功能码	01
            // 操作码	00
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
            //功能码	03
            //子功能码	01
            //状态码	00 解锁 01 锁定
            //CRC校验	2bytes
            //response = "f3,3,1,0,c2,f0";
            var datas = response.Split(",");
            if (datas.Length == 0 || datas[0] != "f3" || datas[1] != "3")
            {
                return null;
            }

            return datas[3] == "0" ? 0 : 1;
        }

    }
}
