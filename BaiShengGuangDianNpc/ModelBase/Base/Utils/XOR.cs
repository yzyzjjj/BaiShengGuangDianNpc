using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ModelBase.Base.Utils
{
    public class XOR
    {
        public static string XorEncode(byte[] str, byte[] key)
        {
            byte[] strbyte = str;
            var strLen = strbyte.Length;
            var keyLen = key.Length;
            var res = new byte[strbyte.Length];
            for (int i = 0; i < strLen; i++)
            {
                var j = i % keyLen;
                res[i] = (byte)(strbyte[i] ^ key[j]);
            }
            string sub = Encoding.UTF8.GetString(res, 0, strLen);
            return sub;
        }
    }
}