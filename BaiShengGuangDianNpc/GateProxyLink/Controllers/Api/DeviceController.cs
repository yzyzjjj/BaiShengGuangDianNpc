using GateProxyLink.Base.Control;
using GateProxyLink.Base.Logic;
using GateProxyLink.Base.Server;
using Microsoft.AspNetCore.Mvc;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GateProxyLink.Controllers.Api
{
    // npc/Device
    [Microsoft.AspNetCore.Mvc.Route("gate/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public DeviceResult GetDevices([FromQuery]string ids)
        {
            var result = new DeviceResult();
            if (ids.IsNullOrEmpty())
            {
                result.datas.AddRange(ServerConfig.ServerManager.GetDevices());
            }
            else
            {
                var idList = ids.Split(",").Select(int.Parse);
                result.datas.AddRange(ServerConfig.ServerManager.GetDevices(idList));
            }
            return result;
        }

        /// <summary>
        /// 获取设备列表
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
        public MessageResult BatchSendMessageBack()
        {
            var param = Request.GetRequestParams();
            var devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(param.GetValue("devicesList"));
            var result = new MessageResult();
            result.messages.AddRange(ServerConfig.ServerManager.SendMessageBack(devicesList));
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

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="deviceInfo"></param>
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

        // POST: api/DeviceLibrary/Upgrade
        [HttpPost("batchUpgrade")]
        public UpgradeResult Upgrade([FromBody] UpgradeInfos upgradeInfos)
        {
            if (upgradeInfos == null || !upgradeInfos.Infos.Any())
            {
                return Result.GenError<UpgradeResult>(Error.ParamError);
            }

            var result = new UpgradeResult { Type = upgradeInfos.Type };
            result.datas.AddRange(ServerConfig.ServerManager.UpgradeClient(upgradeInfos));
            return result;
        }
    }
}