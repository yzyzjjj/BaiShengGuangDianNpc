using System;
using System.Collections.Generic;
using System.Linq;
using GateProxyLinkServer.Base.Control;
using GateProxyLinkServer.Base.Logic;
using GateProxyLinkServer.Base.Server;
using Microsoft.AspNetCore.Mvc;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using Newtonsoft.Json;

namespace GateProxyLinkServer.Controllers.Api
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
        public DeviceResult GetDevices()
        {
            var result = new DeviceResult();
            result.datas.AddRange(ServerConfig.ServerManager.GetDevices());
            return result;
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <returns></returns>
        [HttpGet("single")]
        public DeviceResult GetDevice(int id)
        {
            var result = new DeviceResult();
            result.datas.Add(ServerConfig.ServerManager.GetDevice(id));
            return result;
        }

        /// <summary>
        /// 批量添加设备
        /// </summary>
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
        /// 批量删除设备
        /// </summary>
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
        /// 批量更新设备
        /// </summary>
        /// <returns></returns>
        [HttpPost("batchupdate")]
        public DataResult BatchUpdateDevice()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new DataResult();
            result.datas.AddRange(ServerConfig.ServerManager.UpdateClient(devicesList));
            return result;
        }

        /// <summary>
        /// 批量发送消息 有返回值
        /// </summary>
        /// <returns></returns>
        [HttpPost("batchsendback")]
        public MessageResult BatchSendMessageBack()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var res = ServerConfig.ServerManager.SendMessageBack(devicesList);
            var result = new MessageResult();
            result.messages.AddRange(res);
            return result;
        }

        /// <summary>
        /// 设置存储
        /// </summary>
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

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("analysis")]
        public DataResult Analysis()
        {
            var param = Request.GetRequestParams();
            var insStr = param.GetValue("ins");
            var msgStr = param.GetValue("msg");
            //0xF3,0x02,0x2C,0x01,0xFF,0x00,0xFF,0x00,0x67,0x12
            var result = new DataResult();
            if (insStr.Length == 0)
            {
                return result;
            }

            insStr = insStr.ToLower();
            if (insStr.Contains("0x"))
            {
                insStr = insStr.Replace("0x", "");
            }

            var insList = insStr.Split(",").Select(x => x.PadLeft(2, '0')).ToArray();
            var val = Convert.ToInt32(insList[3] + insList[2], 16);
            var ins = Convert.ToInt32(insList[5] + insList[4], 16);
            var outs = Convert.ToInt32(insList[7] + insList[6], 16);

            var msg = new DeviceInfoMessagePacket(val, ins, outs);
            var t = msg.Deserialize(msgStr);
            if (t != null)
            {
                result.datas.Add(t.vals);
                result.datas.Add(t.ins);
                result.datas.Add(t.outs);
            }
            return result;
        }

    }
}