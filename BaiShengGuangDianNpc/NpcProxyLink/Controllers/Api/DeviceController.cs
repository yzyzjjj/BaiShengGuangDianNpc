using Microsoft.AspNetCore.Mvc;
using ModelBase.Base.Logger;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using NpcProxyLink.Base.Server;
using System;
using System.Collections.Generic;
using ModelBase.Base.EnumConfig;

namespace NpcProxyLink.Controllers.Api
{
    // npc/device
    [Route("npc/device")]
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
            result.datas.AddRange(ServerConfig.ClientManager.GetDevices());
            return result;
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public Result AddDevice([FromBody]DeviceInfo deviceInfo)
        {
            var res = ServerConfig.ClientManager.AddClient(deviceInfo);
            var result = new Result
            {
                errno = res ? Error.Success : Error.DeviceIsExist
            };
            Log.DebugFormat("添加设备,{0},{1},{2}", deviceInfo.Ip, deviceInfo.Port, res);
            return result;
        }

        /// <summary>
        /// 批量添加设备
        /// </summary>
        /// <param name="devicesList"></param>
        /// <returns></returns>
        [HttpPost("batchadd")]
        public DataErrResult BatchAddDevice([FromBody] List<DeviceInfo> devicesList)
        {
            var result = new DataErrResult();
            result.datas.AddRange(ServerConfig.ClientManager.AddClient(devicesList));
            return result;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        public Result DelDevice([FromBody]DeviceInfo deviceInfo)
        {
            var res = ServerConfig.ClientManager.DelClient(deviceInfo);
            var result = new Result
            {
                errno = res ? Error.Success : Error.DeviceIsExist
            };
            return result;
        }

        /// <summary>
        /// 批量删除设备
        /// </summary>
        /// <param name="devicesList"></param>
        /// <returns></returns>
        [HttpPost("batchdelete")]
        public DataErrResult BatchDelDevice([FromBody] List<DeviceInfo> devicesList)
        {
            var result = new DataErrResult();
            result.datas.AddRange(ServerConfig.ClientManager.DelClient(devicesList));
            return result;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("send")]
        public Result SendMessage([FromBody]DeviceInfo deviceInfo)
        {
            var result = new Result
            {
                errno = ServerConfig.ClientManager.SendMessage(deviceInfo)
            };
            return result;
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="devicesList"></param>
        /// <returns></returns>
        [HttpPost("batchsend")]
        public DataErrResult BatchSendMessage([FromBody] List<DeviceInfo> devicesList)
        {
            var result = new DataErrResult();
            result.datas.AddRange(ServerConfig.ClientManager.SendMessage(devicesList));
            return result;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("sendback")]
        public MessageResult SendMessageBack([FromBody]DeviceInfo deviceInfo)
        {
            var msg = ServerConfig.ClientManager.SendMessageBack(deviceInfo);
            var result = new MessageResult();
            result.messages.Add(new Tuple<int, string>(deviceInfo.DeviceId, msg));
            return result;
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="devicesList"></param>
        /// <returns></returns>
        [HttpPost("batchsendback")]
        public MessageResult BatchSendMessageBack([FromBody] List<DeviceInfo> devicesList)
        {
            var result = new MessageResult();
            result.messages.AddRange(ServerConfig.ClientManager.SendMessageBack(devicesList));
            return result;
        }

        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("setstorage")]
        public Result SetStorage([FromBody]DeviceInfo deviceInfo)
        {
            var result = new Result
            {
                errno = ServerConfig.ClientManager.SetStorage(deviceInfo)
            };
            return result;
        }

        /// <summary>
        /// 设置存储
        /// </summary>
        /// <param name="devicesList"></param>
        /// <returns></returns>
        [HttpPost("batchsetstorage")]
        public DataErrResult BatchSetStorage([FromBody] List<DeviceInfo> devicesList)
        {
            var result = new DataErrResult();
            result.datas.AddRange(ServerConfig.ClientManager.SetStorage(devicesList));
            return result;
        }

        /// <summary>
        /// 设置设备监控频率
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        [HttpPost("setfrequency")]
        public Result SetFrequency([FromBody]DeviceInfo deviceInfo)
        {
            var result = new Result
            {
                errno = ServerConfig.ClientManager.SetFrequency(deviceInfo)
            };
            return result;
        }

        /// <summary>
        /// 设置设备监控频率
        /// </summary>
        /// <param name="devicesList"></param>
        /// <returns></returns>
        [HttpPost("batchsetfrequency")]
        public DataErrResult BatchSetFrequency([FromBody] List<DeviceInfo> devicesList)
        {
            var result = new DataErrResult();
            result.datas.AddRange(ServerConfig.ClientManager.SetFrequency(devicesList));
            return result;
        }
    }
}