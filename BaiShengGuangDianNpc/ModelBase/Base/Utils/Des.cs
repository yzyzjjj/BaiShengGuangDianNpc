using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ModelBase.Base.Utils
{

    public class Des
    {
        private static string promotiomKey = "5ik0xu87";
        public static string DesEncode(string str)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(str);
            //建立加密对象的密钥和偏移量
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法
            //使得输入密码必须输入英文文本
            des.Key = Encoding.ASCII.GetBytes(promotiomKey);
            des.IV = Encoding.ASCII.GetBytes(promotiomKey);
            //创建其支持存储区为内存的流
            MemoryStream ms = new MemoryStream();
            //将数据流链接到加密转换的流
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream 
            //(It  will  end  up  in  the  memory  stream) 
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            //用缓冲区的当前状态更新基础数据源或储存库，随后清除缓冲区
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string 
            byte[] encryptData = ms.ToArray();
            return Convert.ToBase64String(encryptData, 0, encryptData.Length);
        }
        public static string DesDecode(string str)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //Put  the  input  string  into  the  byte  array 
            byte[] inputByteArray = Convert.FromBase64String(str);

            //建立加密对象的密钥和偏移量，此值重要，不能修改
            des.Key = Encoding.ASCII.GetBytes(promotiomKey);
            des.IV = Encoding.ASCII.GetBytes(promotiomKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream 
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

    }
}