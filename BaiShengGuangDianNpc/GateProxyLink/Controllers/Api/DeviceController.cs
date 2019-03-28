using GateProxyLink.Base.Logic;
using GateProxyLink.Base.Server;
using Microsoft.AspNetCore.Mvc;
using ModelBase.Base.ServerConfig.Enum;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GateProxyLink.Controllers.Api
{
    // npc/Device
    [Route("gate/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public DataResult GetDevices()
        {
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.GetDevices());
            return result;
        }

        /// <summary>
        /// 重新获取设备列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("reloadlist")]
        public Result ReloadDevices()
        {
            ServerManager.LoadClients();
            return Result.GenError<Result>(Error.Success);
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public Result AddDevice()
        {
            var param = Request.GetRequestParams();
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(param.GetValue("deviceInfo"));
            var res = ServerManager.AddClient(new List<DeviceInfo> { deviceInfo });
            var result = new Result
            {
                errno = res.Any() ? res.First().errno : Error.Fail
            };

            return result;
        }

        /// <summary>
        /// 批量添加设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("batchadd")]
        public DataResult BatchAddDevice()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerManager.AddClient(devicesList));
            return result;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public Result DelDevice()
        {
            var param = Request.GetRequestParams();
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(param.GetValue("deviceInfo"));
            var res = ServerConfig.ServerManager.DelClient(new List<DeviceInfo> { deviceInfo });
            var result = new Result
            {
                errno = res.Any() ? res.First().errno : Error.Fail
            };
            return result;
        }

        /// <summary>
        /// 批量删除设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("batchdelete")]
        public DataResult BatchDelDevice()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.DelClient(devicesList));
            return result;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("send")]
        public Result SendMessage()
        {
            var param = Request.GetRequestParams();
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(param.GetValue("deviceInfo"));
            var res = ServerConfig.ServerManager.SendMessage(new List<DeviceInfo> { deviceInfo });
            var result = new Result
            {
                errno = res.Any() ? res.First().errno : Error.Fail
            };

            return result;
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("batchsend")]
        public DataResult BatchSendMessage()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.SendMessage(devicesList));
            return result;
        }

        /// <summary>
        /// 发送消息 有返回值
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("sendback")]
        public MessageResult SendMessageBack()
        {
            var param = Request.GetRequestParams();
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(param.GetValue("deviceInfo"));
            var res = ServerConfig.ServerManager.SendMessageBack(new List<DeviceInfo> { deviceInfo });
            var result = new MessageResult();
            result.messages.AddRange(res);
            return result;
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("batchsendback")]
        public DataResult BatchSendMessageBack()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.SendMessage(devicesList));
            return result;
        }

        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("setstorage")]
        public Result SetStorage()
        {
            var param = Request.GetRequestParams();
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(param.GetValue("deviceInfo"));
            var res = ServerConfig.ServerManager.SetStorage(new List<DeviceInfo> { deviceInfo });
            var result = new Result
            {
                errno = res.Any() ? res.First().errno : Error.Fail
            };
            return result;
        }

        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("batchsetstorage")]
        public DataResult BatchSetStorage()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.SetStorage(devicesList));

            return result;
        }

        /// <summary>
        /// 设置设备监控频率
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("setfrequency")]
        public Result SetFrequency()
        {
            var param = Request.GetRequestParams();
            var deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(param.GetValue("deviceInfo"));
            var res = ServerConfig.ServerManager.SetFrequency(new List<DeviceInfo> { deviceInfo });
            var result = new Result
            {
                errno = res.Any() ? res.First().errno : Error.Fail
            };

            return result;
        }

        /// <summary>
        /// 设置设备监控频率
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("batchsetfrequency")]
        public DataResult BatchSetFrequency()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.SetFrequency(devicesList));
            return result;
        }
    }
}