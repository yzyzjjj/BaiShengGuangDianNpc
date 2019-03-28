using System;
using ModelBase.Base.Logger;

namespace ModelBase.Base.Utils
{
    /// <summary>
    /// Base64 的摘要说明
    /// </summary>
    public class Base64
    {
        public static string Base64Encode(string str)
        {
            try
            {
                System.Text.Encoding encode = System.Text.Encoding.ASCII;
                byte[] bytedata = encode.GetBytes(str);
                string res = Convert.ToBase64String(bytedata, 0, bytedata.Length);
                return res;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Base64Encode error:{0},str:{1}", e, str);
                return "";
            }
        }

        public static string Base64Decode(string str)
        {
            try
            {
                byte[] bpath = Convert.FromBase64String(str);
                string res = System.Text.Encoding.Default.GetString(bpath);
                return res;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Base64Encode error:{0},str:{1}", e, str);
                return "";
            }
        }

        public static byte[] Base64DecodeBytes(string str)
        {
            byte[] bpath = Convert.FromBase64String(str);
            return bpath;
        }
    }
}