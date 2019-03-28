using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelBase.Base.Utils
{
    public static class HttpHelper
    {
        public static Dictionary<string, string> GetRequestParams(this HttpRequest request)
        {
            return request.Method.ToUpper() == "GET" ? request.Query.ToDictionary(x => x.Key, x => x.Value.ToString()) : request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
        }

        /// <summary>
        /// 判断是否是微信浏览器
        /// </summary>
        /// <returns></returns>
        public static bool IsWeixinBrowser(this HttpRequest request)
        {
            return request.Headers.ContainsKey("UserAgent")
                && request.Headers["UserAgent"].Contains("micromessenger");
        }

        #region 取客户端真实IP

        ///  <summary>  
        ///  取得客户端真实IP。如果有代理则取第一个非内网地址  
        ///  </summary>  
        //        public static string GetIPAddress
        //        {
        //#if DEBUG
        //            get { return "60.190.47.14"; }//公司外网地址
        //#else
        //            get
        //            {
        //                var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    //可能有代理  
        //                    if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
        //                        result = null;

        //                    else
        //                    {
        //                        if (result.IndexOf(",") != -1)
        //                        {
        //                            //有“,”，估计多个代理。取第一个不是内网的IP。  
        //                            result = result.Replace(" ", "").Replace("'", "");
        //                            string[] temparyip = result.Split(',');
        //                            for (int i = 0; i < temparyip.Length; i++)
        //                            {
        //                                if (IsIPAddress(temparyip[i])

        //                                 && temparyip[i].Substring(0, 3) != "10."
        //                                        && temparyip[i].Substring(0, 7) != "192.168"
        //                                        && temparyip[i].Substring(0, 7) != "172.16.")
        //                                {
        //                                    return temparyip[i];        //找到不是内网的地址  
        //                                }
        //                            }
        //                        }
        //                        else if (IsIPAddress(result))  //代理即是IP格式

        //                            return result;
        //                        else
        //                            result = null;        //代理中的内容  非IP，取IP  
        //                    }

        //                }

        //                //string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

        //                if (string.IsNullOrEmpty(result))
        //                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];

        //                if (string.IsNullOrEmpty(result))
        //                    result = HttpContext.Current.Request.UserHostAddress;

        //                return result;
        //            }
        //#endif
        //        }
        #endregion

        #region  判断是否是IP格式

        ///  <summary>
        ///  判断是否是IP地址格式  0.0.0.0
        ///  </summary>
        ///  <param  name="str1">待判断的IP地址</param

        ///  <returns>true  or  false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15)
            {
                return false;
            }

            const string regFormat = @"^\d{1,3}[.]\d{1,3}[.]\d{1,3}[.]\d{1,3}$";

            var regex = new Regex(regFormat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        #endregion

        /// <summary>
        /// 创建一个只有文本的页面
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpResponseMessage GenTextPage(string content)
        {
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(content, Encoding.GetEncoding("UTF-8"), "text/html")
            };
            return result;
        }

        public static RedirectResult RedirectPage(string url)
        {
            var result = new RedirectResult(url);
            return result;
        }

        public static HttpResponseMessage GenEmptyPage()
        {
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent("", Encoding.GetEncoding("UTF-8"), "text/html")
            };
            return result;
        }
    }
}