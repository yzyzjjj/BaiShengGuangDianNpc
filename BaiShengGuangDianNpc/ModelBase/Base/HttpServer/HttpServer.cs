using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using Newtonsoft.Json.Linq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelBase.Base.HttpServer
{
    public class HttpServer
    {
        public HttpServer()
        {

        }

        public static string Result(string account, string url, string verb, Dictionary<string, string> data = null)
        {
            try
            {
                if (!EnumHelper.TryParseStr(verb, out HttpVerb httpVerb))
                {
                    Log.ErrorFormat("请求服务器异常:{0},Verb:{1}", url, verb);
                    return "fail";
                }

                var httpClient = new HttpClient(url, account) { Verb = httpVerb };
                if (data != null)
                {
                    foreach (var dt in data)
                    {
                        httpClient.PostingData.Add(dt.Key, dt.Value);
                    }
                }

                var result = httpClient.GetString();

                return result;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("请求服务器异常:{0},Verb:{1},详情:{2}", url, verb, e.Message);
                return "fail";
            }
        }
        public static string Result(string account, string url, string verb, string rawData = "")
        {
            try
            {
                if (!EnumHelper.TryParseStr(verb, out HttpVerb httpVerb))
                {
                    Log.ErrorFormat("请求服务器异常:{0},verb:{1}", url, verb);
                    return "fail";
                }

                if (!rawData.IsNullOrEmpty())
                {
                    try
                    {
                        var requestBody = JObject.Parse(rawData);
                        if (requestBody.GetValue("id") != null)
                        {
                            url += "/" + requestBody["id"];
                            requestBody.Remove("id");
                            rawData = requestBody.HasValues ? requestBody.ToJSON() : "";
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                var httpClient = new HttpClient(url, account) { Verb = httpVerb };
                httpClient.RawData = rawData;
                var result = httpClient.GetString();

                return result;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("请求服务器异常:{0},Verb:{1},详情:{2}", url, verb, e.Message);
                return "fail";
            }
        }

        public delegate void ResultCallBack(string result, Exception e);
        public static void ResultAsync(string account, string url, string verb, Dictionary<string, string> data = null, PostCallBack callBack = null)
        {
            if (!EnumHelper.TryParseStr(verb, out HttpVerb httpVerb))
            {
                Log.ErrorFormat("请求服务器异常:{0},Verb:{1}", url, verb);
                callBack?.Invoke("", new Exception("Verb Error:" + verb));
            }
            var httpClient = new HttpClient(url, account) { Verb = httpVerb };
            if (data != null)
            {
                foreach (var dt in data)
                {
                    httpClient.PostingData.Add(dt.Key, dt.Value);
                }
            }

            httpClient.AsyncGetString((ss, e) =>
            {
                if (e == null)
                {
                    Log.DebugFormat("PostAsync return:{0}", ss);
                    callBack?.Invoke(ss, null);
                }
                else
                {
                    Log.ErrorFormat("请求服务器异常:{0},Verb:{1},详情:{2}", url, verb, e.Message);
                }
            });
        }
        public static void ResultAsync(string account, string url, string verb, string rawData = "", PostCallBack callBack = null)
        {
            if (!EnumHelper.TryParseStr(verb, out HttpVerb httpVerb))
            {
                Log.ErrorFormat("请求服务器异常:{0},Verb:{1}", url, verb);
                callBack?.Invoke("", new Exception("Verb Error:" + verb));
            }
            if (!rawData.IsNullOrEmpty())
            {
                try
                {
                    var requestBody = JObject.Parse(rawData);
                    if (requestBody.GetValue("id") != null)
                    {
                        url += "/" + requestBody["id"];
                        requestBody.Remove("id");
                        rawData = requestBody.HasValues ? requestBody.ToJSON() : "";
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            var httpClient = new HttpClient(url, account) { Verb = httpVerb, RawData = rawData };
            httpClient.AsyncGetString((ss, e) =>
            {
                if (e == null)
                {
                    Log.DebugFormat("PostAsync return:{0}", ss);
                    callBack?.Invoke(ss, null);
                }
                else
                {
                    Log.ErrorFormat("请求服务器异常:{0},Verb:{1},详情:{2}", url, verb, e.Message);
                }
            });
        }



        public static string Get(string url, Dictionary<string, string> data = null)
        {
            try
            {
                var dts = new ArrayOfString();
                if (data != null)
                {
                    dts.AddRange(data.Select(dt => string.Format("{0}={1}", dt.Key, dt.Value)));
                }

                if (dts.Count > 0)
                {
                    url += "?" + dts.Join("&");
                }
                var httpClient = new HttpClient(url) { Verb = HttpVerb.GET };
                var result = httpClient.GetString();
                return result;

            }
            catch (Exception e)
            {
                Log.ErrorFormat("请求服务器异常 Get:{0},详情:{1}", url, e);
                return "fail";
            }
        }
        public delegate void GetCallBack(string result, Exception e);
        public static void GetAsync(string url, Dictionary<string, string> data = null, GetCallBack callBack = null)
        {
            var dts = new ArrayOfString();
            if (data != null)
            {
                dts.AddRange(data.Select(dt => string.Format("{0}={1}", dt.Key, dt.Value)));
            }

            if (dts.Count > 0)
            {
                url += "?" + dts.Join("&");
            }
            var httpClient = new HttpClient(url) { Verb = HttpVerb.GET };
            httpClient.AsyncGetString((ss, e) =>
            {
                if (e == null)
                {
                    Log.DebugFormat("GetAsync return:{0}", e == null ? ss : e.Message);
                    callBack?.Invoke(ss, e);
                }
                else
                {
                    Log.ErrorFormat("请求服务器异常 GetAsync:{0},详情:{1}", url, e);
                }
            });
        }
        public static string Post(string url, Dictionary<string, string> data)
        {
            try
            {
                var httpClient = new HttpClient(url) { Verb = HttpVerb.POST };
                foreach (var dt in data)
                {
                    httpClient.PostingData.Add(dt.Key, dt.Value);
                }

                var result = httpClient.GetString();

                return result;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("请求服务器异常 Post:{0},详情:{1}", url, e);
                return "fail";
            }
        }
        public static string Post(string url, string rawData)
        {
            try
            {
                var httpClient = new HttpClient(url) { Verb = HttpVerb.POST };
                httpClient.RawData = rawData;
                var result = httpClient.GetString();

                return result;
            }
            catch (Exception e)
            {
                Log.ErrorFormat("请求服务器异常 Post:{0},详情:{1}", url, e);
                return "fail";
            }
        }
        public delegate void PostCallBack(string result, Exception e);
        public static void PostAsync(string url, Dictionary<string, string> data, PostCallBack callBack = null)
        {
            var httpClient = new HttpClient(url) { Verb = HttpVerb.POST };
            foreach (var dt in data)
            {
                httpClient.PostingData.Add(dt.Key, dt.Value);
            }

            httpClient.AsyncGetString((ss, e) =>
            {
                if (e == null)
                {
                    Log.DebugFormat("PostAsync return:{0}", e == null ? ss : e.Message);
                    callBack?.Invoke(ss, e);
                }
                else
                {
                    Log.ErrorFormat("请求服务器异常 PostAsync:{0},详情:{1}", url, e);
                }
            });
        }
        public static void PostAsync(string url, string rawData, PostCallBack callBack = null)
        {
            var httpClient = new HttpClient(url) { Verb = HttpVerb.POST };
            httpClient.RawData = rawData;
            httpClient.AsyncGetString((ss, e) =>
            {
                if (e == null)
                {
                    Log.DebugFormat("PostAsync return:{0}", e == null ? ss : e.Message);
                    callBack?.Invoke(ss, e);
                }
                else
                {
                    Log.ErrorFormat("请求服务器异常 PostAsync:{0},详情:{1}", url, e);
                }
            });
        }
    }
}
