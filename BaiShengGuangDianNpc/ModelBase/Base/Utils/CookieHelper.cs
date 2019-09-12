using Microsoft.AspNetCore.Http;
using System;

namespace ModelBase.Base.Utils
{
    public static class CookieHelper
    {
        /// <summary>
        /// 从请求中获取cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="req"></param>
        public static string GetCookie(string key, HttpRequest req)
        {
            if (req.Cookies.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// 删除浏览器中的cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="resp"></param>
        public static void DelCookie(string key, HttpResponse resp)
        {
            resp.Cookies.Delete(key);
        }

        /// <summary>
        /// 设置cookie数据，默认有效时间是，浏览器会话结束，一般是关闭浏览器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="resp"></param>
        /// <param name="expireSecond">有效时间，单位秒</param>
        public static void SetCookie(string key, string value, HttpResponse resp, int expireSecond = 0)
        {
            DelCookie(key, resp);

            if (expireSecond > 0)
            {
                resp.Cookies.Append(key, value, new CookieOptions
                {
                    Expires = DateTime.Now.AddSeconds(expireSecond)
                });
            }
            else
            {
                resp.Cookies.Append(key, value);
            }
        }

    }
}