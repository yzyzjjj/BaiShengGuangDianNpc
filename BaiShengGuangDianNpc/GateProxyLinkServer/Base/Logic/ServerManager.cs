using GateProxyLinkServer.Base.Server;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Logic;
using ModelBase.Base.UrlMappings;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using ModelBase.Models.Socket;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GateProxyLinkServer.Base.Logic
{
    public class ServerManager : IManager
    {
        public static ConcurrentDictionary<string, NpcSocketMsg> NpcSocketMsgs = new ConcurrentDictionary<string, NpcSocketMsg>();

        // deviceId, client
        private static ConcurrentDictionary<int, DeviceInfo> _clients = new ConcurrentDictionary<int, DeviceInfo>();
        private static List<ClientSocket> _clientSockets = new List<ClientSocket>();

        private static Timer _checkTimer;
        private static Timer _clientTimer;

        private static int _mPort = 9999;
        private static Socket _mServerSocket;//服务器socket

        public void Init()
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, _mPort);
            //创建服务器Socket对象，并设置相关属性  
            _mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定ip和端口  
            _mServerSocket.Bind(ipEndPoint);
            //设置最长的连接请求队列长度  
            _mServerSocket.Listen(50);
            Console.WriteLine("启动监听{0}成功", _mServerSocket.LocalEndPoint);
            //在新线程中监听客户端的连接
            var thread = new Thread(ClientConnectListen) { IsBackground = true };
            thread.Start();

            _checkTimer = new Timer(CheckClientState, null, 10000, 2000);
            _clientTimer = new Timer(GetFromServer, null, 5000, 2000);
        }

        private static void ClientConnectListen()
        {
            while (true)
            {
                //为新的客户端连接创建一个Socket对象  
                var socket = _mServerSocket.Accept();
                var clientSocket = new ClientSocket(socket);
                _clientSockets.Add(clientSocket);
                var log = $"ClientSockets Count:{_clientSockets.Count}";
                Console.WriteLine(log);
                Log.InfoFormat(log);
                Thread.Sleep(1);
            }
        }

        public void LoadConfig()
        {
            //LoadServer();
            //LoadClients();
            //_isInit = true;
        }

        private static void GetFromServer(object state)
        {
            GetFromServer();
        }

        private static void GetFromServer(IEnumerable<int> serverList = null)
        {
            var clientSockets = serverList != null && serverList.Any() ? _clientSockets.Where(x => serverList.Contains(x.ServerId)) : _clientSockets;
            for (var index = clientSockets.Count() - 1; index >= 0; index--)
            {
                var clientSocket = _clientSockets[index];
                var successMsg = new NpcSocketMsg
                {
                    MsgType = NpcSocketMsgType.List,
                };

                clientSocket.Send(successMsg);
            }
        }

        private static void CheckClientState(object obj)
        {
            for (var index = 0; index < _clientSockets.Count; index++)
            {
                var clientSocket = _clientSockets[index];
                if (clientSocket.SocketState == SocketState.Connected
                    && (DateTime.Now - clientSocket.LastReceiveTime).TotalSeconds > 30)
                {
                    var log = $"CheckClientState 超时:{clientSocket.ServerId}";
                    Console.WriteLine(log);
                    Log.InfoFormat(log);
                    clientSocket.SocketState = SocketState.Close;
                }
                if (clientSocket.SocketState == SocketState.Connected)
                {
                    if (clientSocket.DeviceInfos.Any())
                    {
                        foreach (var deviceInfo in clientSocket.DeviceInfos)
                        {
                            var deviceId = deviceInfo.DeviceId;

                            if (_clients.ContainsKey(deviceId))
                            {
                                _clients[deviceId] = deviceInfo;
                            }
                            else
                            {
                                _clients.TryAdd(deviceId, deviceInfo);
                            }
                        }
                    }
                }
                else
                {
                    if (clientSocket.ServerId != 0)
                    {
                        var clients = _clients.Where(x => x.Value.ServerId == clientSocket.ServerId);
                        if (clients.Any())
                        {
                            foreach (var client in _clients)
                            {
                                _clients.Remove(client.Key, out _);
                            }
                        }
                    }

                    clientSocket.Dispose();
                    _clientSockets.Remove(clientSocket);
                }
            }

            //LoadClients();
        }

        #region device 
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeviceInfo> GetDevices(IEnumerable<int> ids = null)
        {
            return ids == null ? _clients.Values.OrderBy(x => x.DeviceId) : _clients.Values.Where(x => ids.Contains(x.DeviceId)).OrderBy(y => y.DeviceId);
        }

        /// <summary>
        /// 获取单个设备
        /// </summary>
        /// <returns></returns>
        public DeviceInfo GetDevice(int id)
        {
            return _clients.Values.FirstOrDefault(x => x.DeviceId == id);
        }

        /// <summary>
        /// 获取添加服务器ID
        /// </summary>
        /// <returns></returns>
        private static int GetAddServer()
        {
            var servers = _clients.Values.GroupBy(x => x.ServerId).ToDictionary(y => y.Key, y => y.Count()).OrderBy(x => x.Value);
            return servers.Any() ? servers.First().Key : 1;
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

                ServerConfig.ApiDb.Execute("UPDATE npc_proxy_link SET `ServerId` = @ServerId WHERE `DeviceId` = @DeviceId;", dealList);

                res.AddRange(SocketResponseErr(dealList, UrlMappings.batchAddDevice, "AddClient", true));
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
                res.AddRange(SocketResponseErr(dealList, UrlMappings.batchDelDevice, "DelClient"));
            }

            return res;
        }

        /// <summary>
        /// 更新设备 根据deviceId
        /// </summary>
        /// <param name="dealList">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<DeviceErr> UpdateClient(IEnumerable<DeviceInfo> dealList)
        {
            var res = new List<DeviceErr>();
            if (dealList.Any())
            {
                res.AddRange(SocketResponseErr(dealList, UrlMappings.batchUpdateDevice, "UpdateClient"));
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
                res.AddRange(SocketResponseErr(dealList, UrlMappings.batchSetStorage, "SetStorage"));
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
                res.AddRange(SocketResponseErr(dealList, UrlMappings.batchSetFrequency, "SetFrequency"));
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
                res.AddRange(SocketResponseStr(dealList, UrlMappings.batchSendBack, "SendMessageBack"));
            }

            return res;
        }

        /// <summary>
        /// 升级设备 根据deviceId
        /// </summary>
        /// <param name="upgradeInfos">待处理列表</param>
        /// <returns></returns>
        public IEnumerable<DeviceErr> UpgradeClient(UpgradeInfos upgradeInfos)
        {
            var res = new List<DeviceErr>();
            if (upgradeInfos.Infos.Any())
            {
                res.AddRange(SocketResponseErr(upgradeInfos));
            }
            return res;
        }
        #endregion

        #region socket

        private static readonly int _tryCount = 60000;
        private static IEnumerable<DeviceErr> SocketResponseErr(IEnumerable<DeviceInfo> dealList, string urlKey, string funName, bool isAdd = false)
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
                    if (!dealList.First(x => x.DeviceId == device.DeviceId).Instruction.IsNullOrEmpty())
                    {
                        device.Instruction = dealList.First(x => x.DeviceId == device.DeviceId).Instruction;
                    }
                }
            }
            else if (funName.Contains("Update"))
            {
                foreach (var device in devicesList)
                {
                    device.Ip = dealList.First(x => x.DeviceId == device.DeviceId).Ip;
                    device.Port = dealList.First(x => x.DeviceId == device.DeviceId).Port;
                }
            }

            var clientSockets = _clientSockets.Where(x => x.SocketState == SocketState.Connected).GroupBy(x => x.ServerId)
                .ToDictionary(x => x.Key, x => x.First());
            //根据serverId分组
            foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
            {
                var serverId = deviceGroup.Key;
                var devices = devicesList.Where(x => x.ServerId == serverId);
                //检查serverId是否存在
                if (!clientSockets.ContainsKey(serverId))
                {
                    res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.NpcServerNotExist)));
                    continue;
                }
                var clientSocket = clientSockets[serverId];
                var guid = StringHelper.CreateGuid();
                var msgType = NpcSocketMsgType.Default;
                switch (urlKey)
                {
                    case "batchAddDevice":
                        msgType = NpcSocketMsgType.Add;
                        break;
                    case "batchDelDevice":
                        msgType = NpcSocketMsgType.Delete;
                        break;
                    case "batchUpdateDevice":
                        msgType = NpcSocketMsgType.Update;
                        break;
                    case "batchSetStorage":
                        msgType = NpcSocketMsgType.Storage;
                        break;
                    case "batchSetFrequency":
                        msgType = NpcSocketMsgType.Frequency;
                        break;
                }

                if (msgType != NpcSocketMsgType.Default)
                {
                    var msg = new NpcSocketMsg
                    {
                        Guid = guid,
                        MsgType = msgType,
                        Body = devices.ToJSON()
                    };
                    clientSocket.Send(msg);
                    var t = 0;
                    NpcSocketMsg npcSocketMsg = null;
                    while (true)
                    {
                        if (NpcSocketMsgs.ContainsKey(guid))
                        {
                            npcSocketMsg = NpcSocketMsgs[guid];
                            NpcSocketMsgs.Remove(guid, out _);
                            break;
                        }
                        if (t > _tryCount)
                        {
                            break;
                        }
                        t++;
                        Thread.Sleep(1);
                    }

                    if (npcSocketMsg != null)
                    {
                        try
                        {
                            var result = JsonConvert.DeserializeObject<DataErrResult>(npcSocketMsg.Body);
                            res.AddRange(result.datas);
                            ServerConfig.ApiDb.Execute("UPDATE npc_proxy_link SET `Storage` = @Storage , `Monitoring` = @Monitoring, `Frequency` = @Frequency, `Instruction` = @Instruction WHERE `DeviceId` = @DeviceId;", devicesList);
                        }
                        catch (Exception e)
                        {
                            res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.AnalysisFail)));
                            Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, npcSocketMsg.Body, e.Message);
                        }
                    }
                    else
                    {
                        res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.Fail)));
                    }
                }
            }
            return res;
        }

        private static IEnumerable<DeviceErr> SocketResponseErr(UpgradeInfos upgradeInfos)
        {
            var res = new List<DeviceErr>();
            //不存在设备列表
            res.AddRange(upgradeInfos.Infos.Where(x => !_clients.ContainsKey(x.DeviceId)).Select(y => new DeviceErr(y.DeviceId, Error.DeviceNotExist)));
            var leftInfos = upgradeInfos.Infos.Where(x => _clients.ContainsKey(x.DeviceId));
            if (leftInfos.Any())
            {
                var devicesList = _clients.Values.Where(x => leftInfos.Any(y => y.DeviceId == x.DeviceId));
                var clientSockets = _clientSockets.Where(x => x.SocketState == SocketState.Connected).GroupBy(x => x.ServerId)
                    .ToDictionary(x => x.Key, x => x.First());
                //根据serverId分组
                foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
                {
                    var serverId = deviceGroup.Key;
                    var devices = devicesList.Where(x => x.ServerId == serverId);
                    //检查serverId是否存在
                    if (!clientSockets.ContainsKey(serverId))
                    {
                        res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.NpcServerNotExist)));
                        continue;
                    }

                    var serverClientInfo = leftInfos.Where(x => devices.Any(y => x.DeviceId == y.DeviceId));
                    var clientSocket = clientSockets[serverId];
                    var guid = StringHelper.CreateGuid();
                    var msgType = NpcSocketMsgType.UpgradeClient;
                    var funName = msgType.ToString();
                    var msg = new NpcSocketMsg
                    {
                        Guid = guid,
                        MsgType = msgType,
                        Body = new UpgradeInfos { Type = upgradeInfos.Type, Infos = serverClientInfo.ToList() }.ToJSON()
                    };
                    Console.WriteLine($"{DateTime.Now} {msg.MsgType} {(serverClientInfo.Select(x => x.DeviceId).ToJSON())} -----------{serverId} Send");
                    clientSocket.Send(msg);
                    var t = 0;
                    var sw = Stopwatch.StartNew();
                    NpcSocketMsg npcSocketMsg = null;
                    while (sw.ElapsedMilliseconds < _tryCount)
                    {
                        if (NpcSocketMsgs.ContainsKey(guid))
                        {
                            npcSocketMsg = NpcSocketMsgs[guid];
                            NpcSocketMsgs.TryRemove(guid, out _);
                            break;
                        }
                        //if (t > _tryCount)
                        //{
                        //    break;
                        //}
                        //t++;
                        Thread.Sleep(1);
                    }

                    if (npcSocketMsg != null)
                    {
                        try
                        {
                            var result = JsonConvert.DeserializeObject<DataErrResult>(npcSocketMsg.Body);
                            res.AddRange(result.datas);
                        }
                        catch (Exception e)
                        {
                            res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.AnalysisFail)));
                            Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, npcSocketMsg.Body, e.Message);
                        }
                    }
                    else
                    {
                        res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.Fail)));
                    }
                }
                if (res.Any(x => x.errno == Error.Success))
                {
                    GetFromServer(clientSockets.Keys);
                }

                //if (res.Any(x => x.errno == Error.Success))
                //{
                //    switch (upgradeInfos.Type)
                //    {
                //        case 1:
                //            foreach (var r in res)
                //            {
                //                _clients[r.DeviceId].DeviceState = DeviceState.UpgradeScript;
                //            }
                //            break;
                //        case 2:
                //            foreach (var r in res)
                //            {
                //                _clients[r.DeviceId].DeviceState = DeviceState.UpgradeFirmware;
                //            }
                //            break;
                //        case 3:
                //            break;
                //        default:
                //            break;
                //    }
                //}
            }
            return res;
        }
        private static IEnumerable<Tuple<int, string>> SocketResponseStr(IEnumerable<DeviceInfo> dealList, string urlKey, string funName)
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

            var clientSockets = _clientSockets.Where(x => x.SocketState == SocketState.Connected).GroupBy(x => x.ServerId)
                .ToDictionary(x => x.Key, x => x.First());
            //根据serverId分组
            foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
            {
                var serverId = deviceGroup.Key;
                var devices = devicesList.Where(x => x.ServerId == serverId);
                //检查serverId是否存在
                if (!clientSockets.ContainsKey(serverId))
                {
                    res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
                    continue;
                }

                var clientSocket = clientSockets[serverId];
                var guid = StringHelper.CreateGuid();
                switch (urlKey)
                {
                    case "batchSendBack":
                        var msg = new NpcSocketMsg
                        {
                            Guid = guid,
                            MsgType = NpcSocketMsgType.SendBack,
                            Body = devices.ToJSON()
                        };
                        clientSocket.Send(msg);
                        break;
                }

                var t = 0;
                NpcSocketMsg npcSocketMsg = null;
                while (true)
                {
                    if (NpcSocketMsgs.ContainsKey(guid))
                    {
                        npcSocketMsg = NpcSocketMsgs[guid];
                        NpcSocketMsgs.Remove(guid, out _);
                        break;
                    }
                    if (t > _tryCount)
                    {
                        break;
                    }
                    t++;
                    Thread.Sleep(1);
                }

                if (npcSocketMsg != null)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<MessageResult>(npcSocketMsg.Body);
                        res.AddRange(result.messages);
                    }
                    catch (Exception e)
                    {
                        res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
                        Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, npcSocketMsg.Body, e.Message);
                    }
                }
                else
                {
                    res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
                }
            }
            return res;
        }

        #endregion

        #region http request

        //private static IEnumerable<DeviceErr> HttpResponseErr(IEnumerable<DeviceInfo> dealList, string urlKey, string funName, bool isAdd = false)
        //{
        //    var res = new List<DeviceErr>();
        //    IEnumerable<DeviceInfo> devicesList;
        //    if (isAdd)
        //    {
        //        //不存在设备列表
        //        devicesList = dealList.Where(x => !_clients.ContainsKey(x.DeviceId));
        //        //存在设备列表
        //        var existList = _clients.Values.Where(x => dealList.Any(y => y.DeviceId == x.DeviceId));
        //        if (existList.Any())
        //        {
        //            res.AddRange(existList.Select(device => new DeviceErr(device.DeviceId, Error.DeviceIsExist)));
        //        }
        //    }
        //    else
        //    {
        //        //不存在设备列表
        //        var notExistList = dealList.Where(x => !_clients.ContainsKey(x.DeviceId));
        //        //存在设备列表
        //        devicesList = _clients.Values.Where(x => dealList.Any(y => y.DeviceId == x.DeviceId));
        //        if (notExistList.Any())
        //        {
        //            res.AddRange(notExistList.Select(device => new DeviceErr(device.DeviceId, Error.DeviceNotExist)));
        //        }
        //    }

        //    if (funName.Contains("Storage"))
        //    {
        //        foreach (var device in devicesList)
        //        {
        //            device.Storage = dealList.First(x => x.DeviceId == device.DeviceId).Storage;
        //        }
        //    }
        //    else if (funName.Contains("Frequency"))
        //    {
        //        foreach (var device in devicesList)
        //        {
        //            device.Monitoring = dealList.First(x => x.DeviceId == device.DeviceId).Monitoring;
        //            device.Frequency = dealList.First(x => x.DeviceId == device.DeviceId).Frequency;
        //            if (!dealList.First(x => x.DeviceId == device.DeviceId).Instruction.IsNullOrEmpty())
        //            {
        //                device.Instruction = dealList.First(x => x.DeviceId == device.DeviceId).Instruction;
        //            }
        //        }
        //    }

        //    //根据serverId分组
        //    foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
        //    {
        //        var serverId = deviceGroup.Key;
        //        var devices = devicesList.Where(x => x.ServerId == serverId);
        //        //检查serverId是否存在
        //        if (!_serversUrl.ContainsKey(serverId))
        //        {
        //            res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.NpcServerNotExist)));
        //            continue;
        //        }
        //        var serverInfo = _serversUrl[serverId];

        //        var url = serverInfo.Url + UrlMappings.Urls[urlKey];
        //        //向NpcProxyLink请求数据
        //        var resp = HttpServer.Post(url, devices.ToJSON());
        //        if (resp == "fail")
        //        {
        //            res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.ExceptionHappen)));
        //            continue;
        //        }

        //        try
        //        {
        //            var result = JsonConvert.DeserializeObject<DataErrResult>(resp);
        //            res.AddRange(result.datas);
        //            LoadClient(serverInfo);
        //            ServerConfig.ApiDb.Execute("UPDATE npc_proxy_link SET `Storage` = @Storage , `Monitoring` = @Monitoring, `Frequency` = @Frequency, `Instruction` = @Instruction WHERE `DeviceId` = @DeviceId;", devicesList);
        //        }
        //        catch (Exception e)
        //        {
        //            res.AddRange(devices.Select(device => new DeviceErr(device.DeviceId, Error.AnalysisFail)));
        //            Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, resp, e.Message);
        //        }
        //    }
        //    return res;
        //}

        //private static IEnumerable<Tuple<int, string>> HttpResponseStr(IEnumerable<DeviceInfo> dealList, string urlKey, string funName)
        //{
        //    var res = new List<Tuple<int, string>>();
        //    IEnumerable<DeviceInfo> devicesList;

        //    //不存在设备列表
        //    var notExistList = dealList.Where(x => !_clients.ContainsKey(x.DeviceId));
        //    //存在设备列表
        //    devicesList = dealList.Where(x => _clients.ContainsKey(x.DeviceId));
        //    if (notExistList.Any())
        //    {
        //        res.AddRange(notExistList.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
        //    }

        //    if (devicesList.Any())
        //    {
        //        foreach (var device in devicesList)
        //        {
        //            device.ServerId = _clients[device.DeviceId].ServerId;
        //        }
        //    }
        //    //根据serverId分组
        //    foreach (var deviceGroup in devicesList.GroupBy(x => x.ServerId))
        //    {
        //        var serverId = deviceGroup.Key;
        //        var devices = devicesList.Where(x => x.ServerId == serverId);
        //        //检查serverId是否存在
        //        if (!_serversUrl.ContainsKey(serverId))
        //        {
        //            res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
        //            continue;
        //        }
        //        var serverInfo = _serversUrl[serverId];

        //        var url = serverInfo.Url + UrlMappings.Urls[urlKey];
        //        //向NpcProxyLink请求数据
        //        var resp = HttpServer.Post(url, devices.ToJSON());
        //        if (resp == "fail")
        //        {
        //            res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
        //            continue;
        //        }

        //        try
        //        {
        //            var result = JsonConvert.DeserializeObject<MessageResult>(resp);
        //            res.AddRange(result.messages);
        //        }
        //        catch (Exception e)
        //        {
        //            res.AddRange(devices.Select(device => new Tuple<int, string>(device.DeviceId, string.Empty)));
        //            Log.ErrorFormat("{0} Res:{1}, Error:{2}", funName, resp, e.Message);
        //        }
        //    }
        //    return res;
        //}

        #endregion

    }
}
