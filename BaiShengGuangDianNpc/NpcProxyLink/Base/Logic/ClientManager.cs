using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Logic;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NpcProxyLink.Base.Server;

namespace NpcProxyLink.Base.Logic
{
    public class ClientManager : IManager
    {
        // deviceId, client
        private static readonly int _period = 50;
        private static readonly int _wc = _period / 2;
        private static ConcurrentDictionary<int, Client> _clients = new ConcurrentDictionary<int, Client>();
        //private static Timer _frequencyTimer = new Timer(FrequencyMonitoring, null, 5000, _period);
        private static Timer _checkTimer = new Timer(CheckClientState, null, 5000, 2000);
        private static bool _isInit;
        private static bool _isRun;
        public void LoadConfig()
        {
            _clients.Clear();
            var deviceInfos = Server.ServerConfig.ApiDb.
                Query<DeviceInfo>("SELECT a.Code, a.Ip, a.`Port`, a.ScriptId, b.`Id`, b.`DeviceId`, b.`ServerId`, b.`GroupId`, b.`Monitoring`, b.`Frequency`, b.`Instruction`, b.`Storage` FROM `device_library` a JOIN `npc_proxy_link` b ON " +
                                  "a.Id = b.DeviceId WHERE a.MarkedDelete = 0 AND b.ServerId = @ServerId;", new
                                  {
                                      Server.ServerConfig.ServerId
                                  });
            foreach (var deviceInfo in deviceInfos)
            {
                //if (deviceInfo.Ip== "192.168.1.16")
                {
                    if (!AddClient(deviceInfo))
                    {
                        Log.ErrorFormat("ClientManager AddClient Fail,{0},{1}:{2}",
                            deviceInfo.DeviceId, deviceInfo.Ip, deviceInfo.Port);
                    }
                }
            }

            _isInit = true;

            Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    if (sw.ElapsedMilliseconds >= 50)
                    {
                        sw.Restart();
                        FrequencyMonitoring();
                    }
                    Thread.Sleep(5);
                }
            });
        }

        public void UpdateConfig()
        {

            var deviceInfos = ServerConfig.ApiDb.
                Query<DeviceInfo>("SELECT b.*, a.* FROM `device_library` a JOIN `npc_proxy_link` b ON " +
                                  "a.Id = b.DeviceId WHERE a.MarkedDelete = 0 AND b.ServerId = @ServerId;", new
                {
                    ServerConfig.ServerId
                });

            foreach (var deviceInfo in deviceInfos)
            {
                var deviceId = deviceInfo.DeviceId;
                if (_clients.ContainsKey(deviceId))
                {
                    _clients[deviceId].Socket.UpdateInfo(deviceInfo);
                }
                else
                {
                    AddClient(deviceInfo);
                }
            }

            foreach (var client in _clients.Values.Where(x => deviceInfos.All(y => y.DeviceId != x.DeviceInfo.DeviceId)))
            {
                client.Dispose();
                _clients.Remove(client.DeviceInfo.DeviceId, out _);
            }
        }

        private static void CheckClientState(object obj)
        {
            if (!_isInit)
            {
                return;
            }
            foreach (var client in _clients)
            {
                var c = client.Value;
                c.Socket?.CheckState();
            }
        }

        /// <summary>
        /// 监控
        /// </summary>
        private static void FrequencyMonitoring()
        {
            var nowTime = DateTime.Now;
            if (!_isInit)
            {
                return;
            }
            if (_isRun)
            {
                return;
            }

            _isRun = true;
            var clients = new List<int>();
            clients.AddRange(_clients.Values.Where(x => x.DeviceInfo != null && x.DeviceInfo.Monitoring && x.DeviceInfo.Frequency > 0 && x.NextSendTime <= nowTime
                                                        && x.Socket.DeviceInfo.State == SocketState.Connected).Select(y => y.DeviceInfo.DeviceId)); ;
            if (clients.Any())
            {
                //Log.Debug("");
                //Log.Debug($"------当前:{nowTime:yyyy-MM-dd HH:mm:ss fff} {clients.Count}------");
                foreach (var deviceId in clients)
                {
                    //var client = _clients[deviceId];
                    if (_clients[deviceId].NextSendTime == default(DateTime))
                    {
                        _clients[deviceId].NextSendTime = nowTime;
                    }
                    //Console.WriteLine($"------本次:{_clients[deviceId].NextSendTime:yyyy-MM-dd HH:mm:ss fff}------{_clients[deviceId].NextSendTime <= nowTime}");
                    _clients[deviceId].NextSendTime = _clients[deviceId].NextSendTime.AddMilliseconds(_clients[deviceId].DeviceInfo.Frequency);
                    //Console.WriteLine($"------下次:{ _clients[deviceId].NextSendTime:yyyy-MM-dd HH:mm:ss fff} {clients.Count}------");
                }

                Parallel.ForEach(clients, (deviceId, state) =>
                {
                    var client = _clients[deviceId];
                    //if (client.NextSendTime == default(DateTime))
                    //{
                    //    client.NextSendTime = nowTime;
                    //}
                    //Log.Debug($"当前:{client.NextSendTime:yyyy-MM-dd HH:mm:ss fff} {clients.Count}");
                    //Log.Debug($"{client.DeviceInfo.Ip} start, 当前:{nowTime:yyyy-MM-dd HH:mm:ss fff}");
                    //Log.Debug($"{client.DeviceInfo.Ip} start, 本次:{client.NextSendTime:yyyy-MM-dd HH:mm:ss fff}");
                    //client.NextSendTime = client.NextSendTime.AddMilliseconds(client.DeviceInfo.Frequency);
                    //Log.Debug($"{client.DeviceInfo.Ip} start, 下次:{client.NextSendTime:yyyy-MM-dd HH:mm:ss fff}");
                    //Log.Debug("");
                    var resError = client.Socket.SendMessageAsync(client.Socket.HeartPacketByte, client.NextSendTime.AddMilliseconds(-_clients[deviceId].DeviceInfo.Frequency));
                    //client.NextSendTime = nowTime.AddMilliseconds(client.DeviceInfo.Frequency > client.Socket.LastConsume ? client.DeviceInfo.Frequency - client.Socket.LastConsume : 0);
                    //Log.Debug($"{client.DeviceInfo.Ip} {resError.GetAttribute<DescriptionAttribute>()?.Description ?? ""}, 当前:{nowTime:yyyy-MM-dd HH:mm:ss fff},下次:{client.NextSendTime:yyyy-MM-dd HH:mm:ss fff}");
                    if (resError == Error.Success)
                    {
                        //ServerConfig.ApiDb.Execute(
                        //    "UPDATE npc_proxy_link SET `Time` = @Time, `State` = 1 WHERE `DeviceId` = @DeviceId;", new
                        //    {
                        //        Time = client.NextSendTime,
                        //        client.DeviceInfo.DeviceId
                        //    });
                    }
                    else if (resError == Error.Fail)
                    {
                        client.NextSendTime = DateTime.Now;
                    }
                    else if (resError == Error.ServerBusy)
                    {
                        //Log.Debug($"{client.DeviceInfo.Ip} busy, 本次:{client.NextSendTime:yyyy-MM-dd HH:mm:ss fff},下次:{client.NextSendTime.AddMilliseconds(client.DeviceInfo.Frequency):yyyy-MM-dd HH:mm:ss fff}");
                        //client.NextSendTime = client.NextSendTime > DateTime.Now ? DateTime.Now : client.NextSendTime;
                    }
                    //Log.Debug(client.DeviceInfo.Ip + " Monitoring success,下次:" + client.NextSendTime.ToString("yyyy-MM-dd HH:mm:ss fff"));

                    //Log.Debug(client.DeviceInfo.Ip + " Monitoring success,发送耗时:" + (DateTime.Now - nowTime).TotalMilliseconds);
                    //Log.Debug("-------------");
                });
            }
            _isRun = false;

        }

        #region device 
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeviceInfo> GetDevices()
        {
            return _clients.Values.Select(x => x.DeviceInfo).OrderBy(x => x.DeviceId);
        }
        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public bool AddClient(DeviceInfo deviceInfo)
        {
            var client = new Client();
            if (!_clients.TryAdd(deviceInfo.DeviceId, client))
            {
                return false;
            }
            client.Init(deviceInfo);

            //if (deviceInfo.Monitoring)
            //{
            //    StartMonitoring(deviceInfo.Frequency);
            //}

            return true;
        }
        public IEnumerable<DeviceErr> AddClient(IEnumerable<DeviceInfo> devicesList)
        {
            var res = new List<DeviceErr>();
            foreach (var device in devicesList)
            {
                var deviceId = device.DeviceId;
                if (_clients.ContainsKey(deviceId))
                {
                    res.Add(new DeviceErr(deviceId, Error.DeviceIsExist));
                }
            }
            foreach (var deviceInfo in devicesList.Where(x => _clients.All(y => y.Key != x.DeviceId)))
            {
                var deviceId = deviceInfo.DeviceId;
                res.Add(new DeviceErr(deviceId, AddClient(deviceInfo) ? Error.Success : Error.DeviceIsExist));
            }

            return res;
        }

        public bool DelClient(DeviceInfo deviceInfo)
        {
            if (_clients.TryRemove(deviceInfo.DeviceId, out var client))
            {
                client.Dispose();
                return true;
            }
            return false;
        }

        public bool UpdateClient(DeviceInfo deviceInfo)
        {
            DelClient(deviceInfo);
            return AddClient(deviceInfo);
        }

        public IEnumerable<DeviceErr> DelClient(IEnumerable<DeviceInfo> devicesList)
        {
            var res = new List<DeviceErr>();
            foreach (var device in devicesList)
            {
                var deviceId = device.DeviceId;
                if (!_clients.ContainsKey(deviceId))
                {
                    res.Add(new DeviceErr(deviceId, Error.DeviceNotExist));
                }
            }
            foreach (var deviceInfo in devicesList.Where(x => _clients.Any(y => y.Key == x.DeviceId)))
            {
                var deviceId = deviceInfo.DeviceId;
                res.Add(new DeviceErr(deviceId, DelClient(deviceInfo) ? Error.Success : Error.DeviceNotExist));
            }

            return res;
        }

        public IEnumerable<DeviceErr> UpdateClient(IEnumerable<DeviceInfo> devicesList)
        {
            var res = new List<DeviceErr>();
            foreach (var device in devicesList)
            {
                var deviceId = device.DeviceId;
                if (!_clients.ContainsKey(deviceId))
                {
                    res.Add(new DeviceErr(deviceId, Error.DeviceNotExist));
                }
            }

            foreach (var deviceInfo in devicesList.Where(x => _clients.Any(y => y.Key == x.DeviceId)))
            {
                var deviceId = deviceInfo.DeviceId;
                res.Add(new DeviceErr(deviceId, UpdateClient(deviceInfo) ? Error.Success : Error.DeviceNotExist));
            }

            return res;
        }

        //public Error SendMessage(DeviceInfo deviceInfo)
        //{
        //    if (!_clients.ContainsKey(deviceInfo.DeviceId))
        //    {
        //        return Error.DeviceNotExist;
        //    }

        //    return _clients[deviceInfo.DeviceId].Socket.SendMessage(deviceInfo.Instruction);
        //}

        //public IEnumerable<DeviceErr> SendMessage(IEnumerable<DeviceInfo> devicesList)
        //{
        //    var res = new List<DeviceErr>();
        //    foreach (var device in devicesList)
        //    {
        //        var deviceId = device.DeviceId;
        //        if (!_clients.ContainsKey(deviceId))
        //        {
        //            res.Add(new DeviceErr(deviceId, Error.DeviceNotExist));
        //        }
        //    }
        //    foreach (var deviceInfo in devicesList.Where(x => _clients.Any(y => y.Key == x.DeviceId)))
        //    {
        //        var deviceId = deviceInfo.DeviceId;
        //        res.Add(new DeviceErr(deviceId, _clients[deviceId].Socket.SendMessage(deviceInfo.Instruction)));
        //    }

        //    return res;
        //}

        public string SendMessageBack(DeviceInfo deviceInfo)
        {
            if (!_clients.ContainsKey(deviceInfo.DeviceId))
            {
                return string.Empty;
            }

            return _clients[deviceInfo.DeviceId].Socket.SendMessageBack(deviceInfo.Instruction);
        }

        public IEnumerable<Tuple<int, string>> SendMessageBack(IEnumerable<DeviceInfo> devicesList)
        {
            var res = new List<Tuple<int, string>>();
            foreach (var device in devicesList)
            {
                var deviceId = device.DeviceId;
                if (!_clients.ContainsKey(deviceId))
                {
                    res.Add(new Tuple<int, string>(deviceId, "DeviceNotExist"));
                }
            }
            foreach (var deviceInfo in devicesList.Where(x => _clients.Any(y => y.Key == x.DeviceId)))
            {
                var deviceId = deviceInfo.DeviceId;
                res.Add(new Tuple<int, string>(deviceId, _clients[deviceId].Socket.SendMessageBack(deviceInfo.Instruction)));
            }

            return res;
        }

        /// <summary>
        /// 设置设备存储数据
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public Error SetStorage(DeviceInfo deviceInfo)
        {
            if (_clients.TryGetValue(deviceInfo.DeviceId, out var client))
            {
                client.DeviceInfo.Storage = deviceInfo.Storage;
                return Error.Success;
            }
            return Error.DeviceNotExist;
        }

        public IEnumerable<DeviceErr> SetStorage(IEnumerable<DeviceInfo> devicesList)
        {
            var res = new List<DeviceErr>();
            foreach (var device in devicesList)
            {
                var deviceId = device.DeviceId;
                if (!_clients.ContainsKey(deviceId))
                {
                    res.Add(new DeviceErr(deviceId, Error.DeviceNotExist));
                }
            }
            foreach (var deviceInfo in devicesList.Where(x => _clients.Any(y => y.Key == x.DeviceId)))
            {
                var deviceId = deviceInfo.DeviceId;
                res.Add(new DeviceErr(deviceId, SetStorage(deviceInfo)));
            }

            return res;
        }


        /// <summary>
        /// 设置设备监控频率
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        public Error SetFrequency(DeviceInfo deviceInfo)
        {
            if (_clients.TryGetValue(deviceInfo.DeviceId, out Client client))
            {
                client.DeviceInfo.Monitoring = deviceInfo.Monitoring;
                client.DeviceInfo.Frequency = deviceInfo.Frequency;
                client.DeviceInfo.Instruction = deviceInfo.Instruction;
                return Error.Success;
            }
            return Error.DeviceNotExist;
        }

        public IEnumerable<DeviceErr> SetFrequency(IEnumerable<DeviceInfo> devicesList)
        {
            var res = new List<DeviceErr>();
            foreach (var device in devicesList)
            {
                var deviceId = device.DeviceId;
                if (!_clients.ContainsKey(deviceId))
                {
                    res.Add(new DeviceErr(deviceId, Error.DeviceNotExist));
                }
            }
            foreach (var deviceInfo in devicesList.Where(x => _clients.Any(y => y.Key == x.DeviceId)))
            {
                var deviceId = deviceInfo.DeviceId;
                res.Add(new DeviceErr(deviceId, SetFrequency(deviceInfo)));
            }

            return res;
        }

        #endregion

    }
}
