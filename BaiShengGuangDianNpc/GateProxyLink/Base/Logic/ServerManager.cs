using GateProxyLink.Base.Server;
using ModelBase.Base.HttpServer;
using ModelBase.Base.Logger;
using ModelBase.Base.Logic;
using ModelBase.Base.UrlMappings;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ModelBase.Base.EnumConfig;

namespace GateProxyLink.Base.Logic
{
    public class ServerManager : IManager
    {
        // deviceId, client
        private static ConcurrentDictionary<int, DeviceInfo> _clients = new ConcurrentDictionary<int, DeviceInfo>();
        private static Dictionary<int, ServerInfo> _serversUrl = new Dictionary<int, ServerInfo>();
        private static Timer _checkTimer = new Timer(CheckClientState, null, 10000, 5000);
        private static bool _isInit;
        public void LoadConfig()
        {
            LoadServer();
            LoadClients();
            _isInit = true;
        }

        /// <summary>
        /// 加载服务器ID
        /// </summary>
        public void LoadServer()
        {
            _serversUrl = Server.ServerConfig.Db.
                Query<ServerInfo>("SELECT * FROM `npc_proxy_link_server`;", new
                {
                    Server.ServerConfig.ServerId
                }).ToDictionary(x => x.ServerId);
        }
        /// <summary>
        /// 加载服务器连接的客户端
        /// </summary>
        public static void LoadClients()
        {
            foreach (var server in _serversUrl)
            {
                server.Value.Normal = false;
                LoadClient(server.Value);
            }
        }

        /// <summary>
        /// 加载单个服务器的客户端
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        public static void LoadClient(ServerInfo serverInfo)
        {
            var url = serverInfo.Url + UrlMappings.Urls["deviceList"];
            //向NpcProxyLink请求数据
            HttpServer.GetAsync(url, null, (resp, exp) =>
            {
                if (resp == "fail")
                {
                    Log.ErrorFormat("ServerManager LoadClient Fail, Server: {0},{1}", serverInfo.ServerId, serverInfo.Url);
                    return;
                }

                try
                {
                    var result = JsonConvert.DeserializeObject<DeviceResult>(resp);
                    var clients = _clients.Where(x => x.Value.ServerId == serverInfo.ServerId);
                    foreach (var client in clients)
                    {
                        _clients.Remove(client.Key, out _);
                    }

                    var devicesList = result.datas;
                    if (!devicesList.Any())
                    {
                        return;
                    }

                    foreach (var deviceInfo in devicesList)
                    {
                        var deviceId = deviceInfo.DeviceId;
                        if (_clients.ContainsKey(deviceId))
                        {
                            var existDeviceInfo = _clients[deviceId];
                            Log.ErrorFormat("ServerManager AddClient Fail, Clients: {0},{1}:{2}, Add: {0},{1}:{2}",
                                existDeviceInfo.DeviceId, existDeviceInfo.Ip, existDeviceInfo.Port,
                                deviceInfo.DeviceId, deviceInfo.Ip, deviceInfo.Port);
                        }
                        else
                        {
                            _clients.TryAdd(deviceId, deviceInfo);
                        }
                    }

                    serverInfo.Normal = true;
                }
                catch (Exception e)
                {
                    Log.ErrorFormat("LoadClient Res:{0}, Error:{1}", resp, e.Message);
                }
            });

        }

        private static void CheckClientState(object obj)
        {
            if (!_isInit)
            {
                return;
            }

            LoadClients();
        }

        #region server
        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServerInfo> GetServers()
        {
            return _serversUrl.Values.OrderBy(x => x.ServerId);
        }

        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        public Error AddServer(ServerInfo serverInfo)
        {
            if (_serversUrl.ContainsKey(serverInfo.ServerId) || serverInfo.Url.IsNullOrEmpty())
            {
                return Error.NpcServerNotExist;
            }

            _serversUrl.Add(serverInfo.ServerId, serverInfo);
            return Error.Success;
        }
        /// <summary>
        /// 获取添加服务器ID
        /// </summary>
        /// <returns></returns>
        private static int GetAddServer()
        {
            var servers = _clients.Values.GroupBy(x => x.ServerId).ToDictionary(y => y.Key, y => y.Count()).OrderBy(z => z.Value);
            return servers.Any() ? servers.First().Key : 1;
        }

        #endregion

        #region device 
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeviceInfo> GetDevices()
        {
            return _clients.Values.OrderBy(x => x.DeviceId);
        }
        public DeviceInfo GetDevice(int id)
        {
            return _clients.Values.FirstOrDefault(x => x.DeviceId == id);
        }
        /// <summary>
        /// 添加新设备  DeviceInfo必须都有
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public static IEnumerable<DeviceErr> AddClient(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<DeviceErr>();
            if (dealList.Any())
            {
                var Sid = GetAddServer();
                foreach (var info in dealList)
                {
                    info.ServerId = Sid;
                }

                ServerConfig.Db.Execute("UPDATE npc_proxy_link SET `ServerId` = @ServerId WHERE `DeviceId` = @DeviceId;", dealList);

                res.AddRange(HttpResponseErr(dealList, "batchAddDevice", "AddClient", true));
            }

            return res;
        }

        /// <summary>
        /// 删除设备 根据deviceId
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<DeviceErr> DelClient(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<DeviceErr>();
            if (dealList.Any())
            {
                res.AddRange(HttpResponseErr(dealList, "batchDelDevice", "DelClient"));
            }

            return res;
        }

        /// <summary>
        /// 设置设备存储数据 根据deviceId
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<DeviceErr> SetStorage(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<DeviceErr>();
            if (dealList.Any())
            {
                res.AddRange(HttpResponseErr(dealList, "batchSetStorage", "SetStorage"));
            }

            return res;
        }

        /// <summary>
        /// 设置设备监控频率 根据deviceId
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<DeviceErr> SetFrequency(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<DeviceErr>();
            if (dealList.Any())
            {
                res.AddRange(HttpResponseErr(dealList, "batchSetFrequency", "SetFrequency"));
            }

            return res;
        }

        /// <summary>
        /// 发送消息 根据deviceId
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<DeviceErr> SendMessage(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<DeviceErr>();
            //指令为空列表
            var instructionErrList = dealList.Where(x => x.Instruction.IsNullOrEmpty());
            if (instructionErrList.Any())
            {
                res.AddRange(instructionErrList.Select(device => new DeviceErr(device.DeviceId, Error.InstructionError)));
            }
            dealList = dealList.Where(x => !x.Instruction.IsNullOrEmpty());
            if (dealList.Any())
            {
                res.AddRange(HttpResponseErr(dealList, "batchSend", "SendMessage"));
            }

            return res;
        }

        /// <summary>
        /// 发送消息 根据deviceId
        /// 有返回值
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<Tuple<int, string>> SendMessageBack(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<Tuple<int, string>>();
            //指令为空列表
            var instructionErrList = dealList.Where(x => x.Instruction.IsNullOrEmpty());
            if (instructionErrList.Any())
            {
                res.AddRange(instructionErrList.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
            }
            dealList = dealList.Where(x => !x.Instruction.IsNullOrEmpty());

            if (dealList.Any())
            {
                res.AddRange(HttpResponseStr(dealList, "batchSendBack", "SendMessageBack"));
            }

            return res;
        }
        #endregion

        #region http request

        private static IEnumerable<DeviceErr> HttpResponseErr(IEnumerable<DeviceInfo> dealList, string urlKey, string funName, bool isAdd = false)
        {
            var res = new List<DeviceErr>();
            IEnumerable<DeviceInfo> devicesList;
            if (isAdd)
            {
                //不存在设备列表
                devicesList = dealList.Where(x => !_clients.ContainsKey(x.DeviceId));
                //存在设备列表
                var existList = _clients.Values.Where(x => dealList.Any(y => y.DeviceId == x.DeviceId));
                if (existList.Any())
                {
                    res.AddRange(existList.Select(device => new DeviceErr(device.DeviceId, Error.DeviceIsExist)));
                }
            }
            else
            {
                //不存在设备列表
                var notExistList = dealList.Where(x => !_clients.ContainsKey(x.DeviceId));
                //存在设备列表
                devicesList = _clients.Values.Where(x => dealList.Any(y => y.DeviceId == x.DeviceId));
                if (notExistList.Any())
                {
                    res.AddRange(notExistList.Select(device => new DeviceErr(device.DeviceId, Error.DeviceNotExist)));
                }
            }

            if (funName.Contains("Storage"))
            {
                foreach (var device in devicesList)
                {
                    device.Storage = dealList.First(x => x.DeviceId == device.DeviceId).Storage;
                }
            }
            else if (funName.Contains("Frequency"))
            {
                foreach (var device in devicesList)
                {
                    device.Monitoring = dealList.First(x => x.DeviceId == device.DeviceId).Monitoring;
                    device.Frequency = dealList.First(x => x.DeviceId == device.DeviceId).Frequency;
                    device.Instruction = dealList.First(x => x.DeviceId == device.DeviceId).Instruction;
                }
            }

            //根据serverId分组
            foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
            {
                var serverId = deviceGroup.Key;
                var devices = devicesList.Where(x => x.ServerId == serverId);
                //检查serverId是否存在
                if (!_serversUrl.ContainsKey(serverId))
                {
                    res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.NpcServerNotExist)));
                    continue;
                }
                var serverInfo = _serversUrl[serverId];

                var url = serverInfo.Url + UrlMappings.Urls[urlKey];
                //向NpcProxyLink请求数据
                var resp = HttpServer.Post(url, devices.ToJSON());
                if (resp == "fail")
                {
                    res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.ExceptionHappen)));
                    continue;
                }

                try
                {
                    var result = JsonConvert.DeserializeObject<DataErrResult>(resp);
                    res.AddRange(result.datas);
                    LoadClient(serverInfo);
                }
                catch (Exception e)
                {
                    res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.AnalysisFail)));
                    Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, resp, e.Message);
                }
            }
            return res;
        }

        private static IEnumerable<Tuple<int, string>> HttpResponseStr(IEnumerable<DeviceInfo> dealList, string urlKey, string funName)
        {
            var res = new List<Tuple<int, string>>();
            IEnumerable<DeviceInfo> devicesList;

            //不存在设备列表
            var notExistList = dealList.Where(x => !_clients.ContainsKey(x.DeviceId));
            //存在设备列表
            devicesList = dealList.Where(x => _clients.ContainsKey(x.DeviceId));
            if (notExistList.Any())
            {
                res.AddRange(notExistList.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
            }

            if (devicesList.Any())
            {
                foreach (var device in devicesList)
                {
                    device.ServerId = _clients[device.DeviceId].ServerId;
                }
            }
            //根据serverId分组
            foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
            {
                var serverId = deviceGroup.Key;
                var devices = devicesList.Where(x => x.ServerId == serverId);
                //检查serverId是否存在
                if (!_serversUrl.ContainsKey(serverId))
                {
                    res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
                    continue;
                }
                var serverInfo = _serversUrl[serverId];

                var url = serverInfo.Url + UrlMappings.Urls[urlKey];
                //向NpcProxyLink请求数据
                var resp = HttpServer.Post(url, devices.ToJSON());
                if (resp == "fail")
                {
                    res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
                    continue;
                }

                try
                {
                    var result = JsonConvert.DeserializeObject<MessageResult>(resp);
                    res.AddRange(result.messages);
                }
                catch (Exception e)
                {
                    res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
                    Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, resp, e.Message);
                }
            }
            return res;
        }

        #endregion

    }
}
