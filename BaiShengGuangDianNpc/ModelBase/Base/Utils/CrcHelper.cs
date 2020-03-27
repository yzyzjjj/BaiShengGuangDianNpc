using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelBase.Base.Utils
{
    public class CrcHelper
    {
        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="data">校验数据</param>
        /// <returns>高低8位</returns>
        public static IEnumerable<string> GetCrc16(IEnumerable<string> data)
        {
            var crcBuf = data.Select(str => byte.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier)).ToArray();
            //计算并填写CRC校验码
            var crc = 0xffff;
            var len = crcBuf.Length;
            for (var n = 0; n < len; n++)
            {
                byte i;
                crc = crc ^ crcBuf[n];
                for (i = 0; i < 8; i++)
                {
                    var tt = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (tt == 1)
                    {
                        crc = crc ^ 0xa001;
                    }
                    crc = crc & 0xffff;
                }
            }

            var res = new[]
            {
                Convert.ToString((byte)(crc & 0xff), 16).PadLeft(2,'0'),
                Convert.ToString((byte)((crc >> 8) & 0xff), 16).PadLeft(2,'0')
            };
            return res;
        }

    }
}