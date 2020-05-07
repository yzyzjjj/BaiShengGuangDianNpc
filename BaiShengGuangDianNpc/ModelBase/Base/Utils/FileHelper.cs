using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelBase.Base.Utils
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 将本地文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static IEnumerable<byte> LocalFileToBytes(string path)
        {
            if (!File.Exists(path))
            {
                return new byte[0];
            }

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var br = new BinaryReader(fs);
                var byData = br.ReadBytes((int)fs.Length);
                return byData;
            }
        }

        /// <summary>
        /// 将网络文件转换为byte数组
        /// </summary>
        /// <param name="url">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static IEnumerable<string> RemoteFileToBytes(string url)
        {
            IEnumerable<byte> data = new List<byte>();
            if (JudgeFileExist(url, out data))
            {
                var binary = data.Select(x => Convert.ToString(x, 16));
                return binary;
            }
            return new string[0];
        }

        /// <summary>
        /// 判断网络文件是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool JudgeFileExist(string url, out IEnumerable<byte> data)
        {
            data = null;
            try
            {
                //创建根据网络地址的请求对象
                var httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.CreateDefault(new Uri(url));
                var response = httpWebRequest.GetResponse();
                var count = (int)response.ContentLength;
                var r = ((System.Net.HttpWebResponse)response).StatusCode == System.Net.HttpStatusCode.OK && count > 0;
                using (var rs = response.GetResponseStream())
                {
                    var offset = 0;
                    var buf = new byte[count];
                    while (count > 0)
                    {
                        if (rs == null)
                        {
                            continue;
                        }

                        var n = rs.Read(buf, offset, count);
                        if (n == 0)
                        {
                            break;
                        }

                        count -= n;
                        offset += n;
                    }
                    data = buf;
                }
                //返回响应状态是否是成功比较的布尔值
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将本地文件转换为16进制string数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static IEnumerable<string> LocalFileTo16String(string path)
        {
            if (!File.Exists(path))
            {
                return new string[0];
            }

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var br = new BinaryReader(fs);
                var byData = br.ReadBytes((int)fs.Length);
                //var binary = byData.Select(x => Convert.ToString(x, 16)).Join(" ");
                var binary = byData.Select(x => Convert.ToString(x, 16));
                return binary;
            }
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到本地指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savePath">保存地址</param>
        public static void BytesToLocalFile(byte[] buff, string savePath)
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            using (var fs = new FileStream(savePath, FileMode.CreateNew))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(buff, 0, buff.Length);
                }
            }
        }
    }
}
