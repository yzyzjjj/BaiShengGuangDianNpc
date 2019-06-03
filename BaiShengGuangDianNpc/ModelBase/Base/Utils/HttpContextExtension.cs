using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Web;

namespace ModelBase.Base.Utils
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取客户端地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取客户端地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIp(this HttpRequest request)
        {
            var ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取请求身份信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIdentityInformation(this HttpRequest request)
        {
            var identityInformation = request.Headers["IdentityInformation"].FirstOrDefault();
            identityInformation = HttpUtility.UrlDecode(identityInformation);
            if (string.IsNullOrEmpty(identityInformation))
            {
                identityInformation = GetIp(request);
            }
            return identityInformation;
        }
    }
}