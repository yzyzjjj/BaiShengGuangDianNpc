using GateProxyLink.Base.Logic;
using GateProxyLink.Base.Server;
using Microsoft.AspNetCore.Mvc;
using ModelBase.Base.ServerConfig.Enum;
using ModelBase.Base.Utils;
using ModelBase.Models.Result;
using Newtonsoft.Json;

namespace GateProxyLink.Controllers.Api
{
    [Route("gate/server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public DataResult GetServers()
        {
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.GetServers());
            return result;
        }

        /// <summary>
        /// 重新获取服务器列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("reloadlist")]
        public Result ReloadDevices()
        {
            ServerConfig.ServerManager.LoadServer();
            return Result.GenError<Result>(Error.Success);
        }

        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public Result AddServer()
        {
            var param = Request.GetRequestParams();
            var serverInfo = JsonConvert.DeserializeObject<ServerInfo>(param.GetValue("serverInfo"));
            var result = new Result
            {
                errno = ServerConfig.ServerManager.AddServer(serverInfo)
            };

            return result;
        }
    }
}