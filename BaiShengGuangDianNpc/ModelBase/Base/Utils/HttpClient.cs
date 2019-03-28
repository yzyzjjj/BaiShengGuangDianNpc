using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ServiceStack;

namespace ModelBase.Base.Utils
{
    public class HttpClient
    {

        public const string ContentType_Form = "application/x-www-form-urlencoded";
        public const string ContentType_Json = "application/json;charset=utf-8";
        #region Properties

        /// <summary>
        /// 是否自动在不同的请求间保留Cookie, Referer
        /// </summary>
        public bool KeepContext { get; set; }

        /// <summary>
        /// 期望的回应的语言
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// GetString()如果不能从HTTP头或Meta标签中获取编码信息,则使用此编码来获取字符串
        /// </summary>
        public Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// 获取或设置 Content-type HTTP 标头的值。
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 指示发出Get请求还是Post请求
        /// </summary>
        public HttpVerb Verb { get; set; }

        /// <summary>
        /// 要发送的Form表单信息
        /// </summary>
        public Dictionary<string, string> PostingData { get; private set; }

        /// <summary>
        /// 要发送的查询字符串信息
        /// </summary>
        public Dictionary<string, string> QueryingData { get; private set; }
        /// <summary>
        /// 要发送的raw信息
        /// </summary>
        public string RawData { get; set; }
        /// <summary>
        /// 获取或设置请求资源的地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置期望的资源类型
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// 获取或设置请求中的Http头User-Agent的值
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置Cookie及Referer
        /// </summary>
        public HttpClientContext Context { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        public HttpClient()
            : this(null)
        {
        }

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        /// <param name="url">要获取的资源的地址</param>
        public HttpClient(string url)
            : this(url, null)
        {
        }

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        /// <param name="url">要获取的资源的地址</param>
        /// <param name="context">Cookie及Referer</param>
        public HttpClient(string url, HttpClientContext context)
            : this(url, context, false)
        {
        }

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        /// <param name="url">要获取的资源的地址</param>
        /// <param name="context">Cookie及Referer</param>
        /// <param name="keepContext">是否自动在不同的请求间保留Cookie, Referer</param>
        public HttpClient(string url, HttpClientContext context, bool keepContext)
        {
            Url = url;
            Context = context;
            KeepContext = keepContext;

            Verb = HttpVerb.GET;
            Accept = "*/*";
            UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            PostingData = new Dictionary<string, string>();
            QueryingData = new Dictionary<string, string>();
            DefaultLanguage = "zh-CN";
            DefaultEncoding = Encoding.UTF8;

            if (Context == null)
            {
                Context = new HttpClientContext();
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 清空PostingData, ResponseHeaders, 并把Verb设置为Get.
        /// 在发出一个包含上述信息的请求后,必须调用此方法或手工设置相应属性以使下一次请求不会受到影响.
        /// </summary>
        public void Reset()
        {
            Verb = HttpVerb.GET;
            PostingData.Clear();
            m_ResponseHeaders = null;
        }

        private HttpWebRequest CreateRequest()
        {
            string url = Url;
            string queryString = null;
            if (QueryingData.Count > 0)
            {
                queryString = string.Join("&", QueryingData.Select(kv => string.Format("{0}={1}", kv.Key, kv.Value)));
            }
            if (queryString == null)
            {
            }
            else if (Url.IndexOf("?") != -1)
            {
                url += "&" + queryString;
            }
            else
            {
                url += "?" + queryString;
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AllowAutoRedirect = false;
            req.CookieContainer = new CookieContainer();
            req.Headers.Add("Accept-Language", DefaultLanguage);
            req.Accept = Accept;
            req.UserAgent = UserAgent;
            req.KeepAlive = false;

            if (Context.Cookies != null)
            {
                req.CookieContainer.Add(Context.Cookies);
            }

            if (!string.IsNullOrEmpty(Context.Referer))
            {
                req.Referer = Context.Referer;
            }

            if (Verb == HttpVerb.HEAD)
            {
                req.Method = "HEAD";
                return req;
            }

            if (PostingData.Count > 0)
            {
                Verb = HttpVerb.POST;
            }

            if (Verb == HttpVerb.POST)
            {
                req.Method = "POST";

                MemoryStream memoryStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memoryStream);


                req.ContentType = ContentType;
                if (string.IsNullOrEmpty(req.ContentType))
                {
                    req.ContentType = ContentType_Form;
                }
                if(!RawData.IsNullOrEmpty())
                    req.ContentType = ContentType_Json;

                if (req.ContentType == ContentType_Form)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string key in PostingData.Keys)
                    {
                        sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(PostingData[key]));
                    }
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                    }

                    writer.Write(sb.ToString());
                }
                else
                {  
                    //raw   ContentType_Json
                    writer.Write(RawData);
                }
                writer.Flush();

                using (Stream stream = req.GetRequestStream())
                {
                    memoryStream.WriteTo(stream);
                }
            }
            return req;
        }

        private ConcurrentDictionary<HttpWebRequest, TaskCompletionSource<AsyncResponseResult>> m_ReuestResponse =
            new ConcurrentDictionary<HttpWebRequest, TaskCompletionSource<AsyncResponseResult>>();

        /// <summary>
        /// 异步发出一次新的请求 
        /// </summary>
        /// <returns>新请求关联的任务</returns>
        public Task<AsyncResponseResult> AsyncGetResponse()
        {
            var request = CreateRequest();
            var respTaskSource = new TaskCompletionSource<AsyncResponseResult>();

            m_ReuestResponse.TryAdd(request, respTaskSource);

            var result = request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);

            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback),
                request, request.Timeout, true);

            return respTaskSource.Task;
        }

        private WebHeaderCollection m_ResponseHeaders;

        /// <summary>
        /// 发出一次新的请求,并返回获得的回应
        /// 调用此方法永远不会触发StatusUpdate事件.
        /// </summary>
        /// <returns>相应的HttpWebResponse</returns>
        public HttpWebResponse GetResponse()
        {
            HttpWebRequest req = CreateRequest();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            m_ResponseHeaders = res.Headers;
            if (KeepContext)
            {
                Context.Cookies = res.Cookies;
                Context.Referer = Url;
            }
            return res;
        }

        /// <summary>
        /// 发出一次新的请求,并返回回应内容的流
        /// 调用此方法永远不会触发StatusUpdate事件.
        /// </summary>
        /// <returns>包含回应主体内容的流</returns>
        public Stream GetStream()
        {
            return GetResponse().GetResponseStream();
        }

        public void AsyncGetStream(Action<Stream, WebHeaderCollection, Exception> callback)
        {
            AsyncGetResponse().ContinueWith(t =>
            {
                var exception = t.Result.Exception;
                if (exception != null)
                {
                    callback(null, null, exception);
                }
                else
                {
                    var response = t.Result.Response;
                    callback(response.GetResponseStream(), response.Headers, null);
                }
            });
        }

        /// <summary>
        /// 发出一次新的请求,并以字节数组形式返回回应的内容
        /// 调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <returns>包含回应主体内容的字节数组</returns>
        public byte[] GetBytes()
        {
            HttpWebResponse res = GetResponse();
            int length = (int)res.ContentLength;

            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[0x100];
            Stream rs = res.GetResponseStream();
            for (int i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
            {
                memoryStream.Write(buffer, 0, i);
            }
            rs.Close();

            return memoryStream.ToArray();
        }

        /// <summary>
        /// 异步发送一次新的请求，以字节数组形式返回相应内容
        /// </summary>
        /// <param name="callback">当请求返回时执行的回调</param>
        public void AsyncGetBytes(Action<byte[], WebHeaderCollection, Exception> callback)
        {
            AsyncGetResponse().ContinueWith(t =>
            {
                var exception = t.Result.Exception;
                if (exception != null)
                {
                    callback(null, null, exception);
                }
                else
                {
                    var response = t.Result.Response;
                    int length = (int)response.ContentLength;

                    MemoryStream memoryStream = new MemoryStream();
                    byte[] buffer = new byte[0x100];
                    Stream rs = response.GetResponseStream();
                    for (int i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
                    {
                        memoryStream.Write(buffer, 0, i);
                    }
                    rs.Close();
                    callback(memoryStream.ToArray(), response.Headers, null);
                }
            });
        }

        /// <summary>
        /// 发出一次新的请求,以Http头,或Html Meta标签,或DefaultEncoding指示的编码信息对回应主体解码
        /// </summary>
        /// <returns>解码后的字符串</returns>
        public string GetString()
        {
            return GetString(GetBytes(), m_ResponseHeaders);
        }

        /// <summary>
        /// 发出一次新的请求,对回应的主体内容以指定的编码进行解码
        /// </summary>
        /// <param name="encoding">指定的编码</param>
        /// <returns>解码后的字符串</returns>
        public string GetString(Encoding encoding)
        {
            return GetString(GetBytes(), encoding);
        }

        /// <summary>
        /// 异步以Http头,或Html Meta标签,或DefaultEncoding指示的编码信息对回应主体解码
        /// </summary>
        /// <param name="callback">当响应返回时执行的回调</param>
        public void AsyncGetString(Action<string, Exception> callback)
        {
            try
            {
                AsyncGetBytes((r, h, e) =>
                {
                    if (e != null)
                    {
                        callback(null, e);
                    }
                    else
                    {
                        callback(GetString(r, h), null);
                    }
                });
            }
            catch (Exception ex)
            {
                callback(null, ex);
            }
        }
        /// <summary>
        /// 发出一次新的请求,对回应的主体内容以指定的编码进行解码
        /// </summary>
        /// <param name="callback">当响应返回时执行的回调</param>
        /// <param name="encoding">指定的编码</param>
        public void AsyncGetString(Action<string, Exception> callback, Encoding encoding)
        {

            AsyncGetBytes((r, h, e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                }
                else
                {
                    callback(GetString(r, encoding), null);
                }
            });
        }
        /// <summary>
        /// 发出一次新的请求,把回应的主体内容保存到文件
        /// 如果指定的文件存在,它会被覆盖
        /// </summary>
        /// <param name="fileName">要保存的文件路径</param>
        public void SaveAsFile(string fileName)
        {
            SaveAsFile(fileName, FileExistsAction.Overwrite);
        }

        /// <summary>
        /// 发出一次新的请求,把回应的主体内容保存到文件
        /// </summary>
        /// <param name="fileName">要保存的文件路径</param>
        /// <param name="existsAction">指定的文件存在时的选项</param>
        /// <returns>是否向目标文件写入了数据</returns>
        public bool SaveAsFile(string fileName, FileExistsAction existsAction)
        {
            byte[] data = GetBytes();
            switch (existsAction)
            {
                case FileExistsAction.Overwrite:
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
                    {
                        writer.Write(data);
                    }

                    return true;

                case FileExistsAction.Append:
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write)))
                    {
                        writer.Write(data);
                    }

                    return true;

                default:
                    if (!File.Exists(fileName))
                    {
                        using (
                            BinaryWriter writer =
                                new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                        {
                            writer.Write(data);
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }
        #endregion

        #region Private Method
        private string GetEncodingFromHeaders(WebHeaderCollection responseHeaders)
        {
            string encoding = null;
            string contentType = responseHeaders["Content-Type"];
            if (contentType != null)
            {
                int i = contentType.IndexOf("charset=");
                if (i != -1)
                {
                    encoding = contentType.Substring(i + 8);
                }
            }
            return encoding;
        }

        private string GetString(byte[] data, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = DefaultEncoding;
            }
            return encoding.GetString(data);
        }

        private string GetString(byte[] data, WebHeaderCollection responseHeaders)
        {
            Encoding encoding;
            string encodingName = GetEncodingFromHeaders(responseHeaders);
            if (encodingName != null)
            {
                encoding = Encoding.GetEncoding(encodingName);
            }
            else
            {
                encoding = DefaultEncoding;
            }

            return encoding.GetString(data);
        }

        private void ResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = null;
            Exception exception = null;
            try
            {
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            }
            catch (Exception e)
            {
                exception = e;
            }
            TaskCompletionSource<AsyncResponseResult> respTaskSource;
            if (m_ReuestResponse.TryRemove(request, out respTaskSource))
            {
                respTaskSource.SetResult(new AsyncResponseResult(response, exception));
            }
        }

        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;

                if (request != null)
                {
                    TaskCompletionSource<AsyncResponseResult> respTaskSource;
                    if (m_ReuestResponse.TryRemove(request, out respTaskSource))
                    {
                        respTaskSource.SetResult(new AsyncResponseResult(null, new TimeoutException("Request timeout")));
                    }
                    request.Abort();
                }
            }
        }
        #endregion
    }

    public class AsyncResponseResult
    {
        public AsyncResponseResult(HttpWebResponse response, Exception exception)
        {
            Response = response;
            Exception = exception;
        }

        public HttpWebResponse Response { get; private set; }
        public Exception Exception { get; private set; }
    }
    public class HttpClientContext
    {
        private CookieCollection cookies;
        private string referer;

        public CookieCollection Cookies
        {
            get { return cookies; }
            set { cookies = value; }
        }

        public string Referer
        {
            get { return referer; }
            set { referer = value; }
        }
    }

    public enum HttpVerb
    {
        GET,
        POST,
        HEAD,
    }

    public enum FileExistsAction
    {
        Overwrite,
        Append,
        Cancel,
    }
}
