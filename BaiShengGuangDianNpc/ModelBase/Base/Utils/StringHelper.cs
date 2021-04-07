using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelBase.Base.Utils
{
    public static class StringHelper
    {
        private static readonly Random _Seed = new Random();
        public static bool IsAnyNullOrWhiteSpace(params string[] args)
        {
            return args.Any(string.IsNullOrWhiteSpace);
        }
        /// <summary>
        /// 创建一个32长度的小写guid
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CreateOrderId(string userid)
        {
            var str = DateTime.Now.ToString("yyMMddHHmmss") + Get4(userid) + CreateRandomNum(4);
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> CreateOrderIdAsc(string userid, int count)
        {
            var strs = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var str = DateTime.Now.ToString("yyMMddHHmmss") + Get4(userid) + string.Format("{0:D3}", i);
                strs.Add(str);
            }
            return strs;
        }


        public static string CreateOrderId(DateTime time, string userid)
        {
            var str = time.ToString("yyMMddHHmmss") + Get4(userid) + CreateRandomNum(4);
            return str;
        }
        private static string Get4(string userid)
        {
            long.TryParse(userid, out var a);
            if (a <= 0)
            {
                return CreateRandomNum(4);
            }

            if (a >= 10000)
            {
                var str = a.ToString();
                return str.Substring(str.Length - 4);
            }
            else
            {
                return a.ToString("D4");
            }
        }

        public static string CreateRandomNum(int n)
        {
            var str = "0123456789";
            var sb = new StringBuilder();
            for (var i = 0; i < n; i++)
            {
                sb.Append(str.Substring(_Seed.Next(0, str.Length), 1));
            }
            return sb.ToString();

        }

        public static string CreateRandomNum2(int n)
        {
            const string str = "0123456789abcdefghijklmopqrstuvwxyz";
            var sb = new StringBuilder();
            for (var i = 0; i < n; i++)
            {
                sb.Append(str.Substring(_Seed.Next(0, str.Length), 1));
            }
            return sb.ToString();
        }

        public static string ToJSON<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToClass<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
        /// <summary>
        /// 比较版本号，若v1>v2 返回true,否则返回false
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool CompareVersion(string v1, string v2)
        {
            try
            {
                var arr1 = v1.Split('.').Select(int.Parse).ToArray();
                var arr2 = v2.Split('.').Select(int.Parse).ToArray();
                for (int i = 0; i < 3; i++)
                {
                    if (arr1[i] > arr2[i])
                    {
                        return true;
                    }
                    if (arr1[i] < arr2[i])
                    {
                        return false;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        public static int CompareVersionInt(string v1, string v2)
        {
            var arr1 = v1.Split('.').Select(int.Parse).ToArray();
            var arr2 = v2.Split('.').Select(int.Parse).ToArray();
            for (int i = 0; i < 3; i++)
            {
                if (arr1[i] > arr2[i])
                {
                    return 1;
                }
                if (arr1[i] < arr2[i])
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// mysql字段单引号转义
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string ToMySqlParam(this string field)
        {
            return field.Replace("'", @"\'");
        }

        public static byte[] AsciiStringToBytes(this string str)
        {
            var strs = str.Split(',');
            var res = new List<byte>();
            foreach (var s in strs)
            {
                if (s != "")
                {
                    res.Add(byte.Parse(s));
                }
            }
            return res.ToArray();
        }

        public static string ToSqlParam(this string field)
        {
            return field.Replace("'", @"\'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ConvertToChinese(this Decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }

        /// <summary>
        /// 拼接字符串 逗号分隔
        /// </summary>
        /// <param name="url"></param>
        /// <param name="newUrl"></param>
        /// <returns></returns>
        public static string ConcatUrl(this string url, string newUrl)
        {
            if (string.IsNullOrEmpty(url))
            {
                return newUrl;
            }

            if (string.IsNullOrEmpty(newUrl))
            {
                return url;
            }

            return string.Concat(url, ",", newUrl);
        }

        public static string SubUrl(this string url, int index)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            var list = url.Split(',').ToList();
            if (list.Count > index && index >= 0)
            {
                list.RemoveAt(index);
            }
            else
            {
                return url;
            }
            return string.Join(",", list);
        }

        private static readonly char[] Constant =
        {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int length)
        {
            var newRandom = new StringBuilder(length);
            var rd = new Random();
            for (var i = 0; i < length; i++)
            {
                newRandom.Append(Constant[rd.Next(Constant.Length)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 字符串翻转
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Reverse(this string text)
        {
            return new string(text.ToCharArray().Reverse().ToArray());
        }
    }
}