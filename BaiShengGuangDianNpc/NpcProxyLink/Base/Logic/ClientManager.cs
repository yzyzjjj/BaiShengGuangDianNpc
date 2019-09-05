﻿using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Logic;
using ModelBase.Models.Device;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NpcProxyLink.Base.Logic
{
    public class ClientManager : IManager
    {
        // deviceId, client
        private static readonly int _period = 50;
        private static readonly int _wc = 25;
        private static ConcurrentDictionary<int, Client> _clients = new ConcurrentDictionary<int, Client>();
        private static Timer _frequencyTimer = new Timer(FrequencyMonitoring, null, 5000, _period);
        private static Timer _checkTimer = new Timer(CheckClientState, null, 5000, 2000);
        private static bool _isInit;
        public void LoadConfig()
        {
            _clients.Clear();
            var deviceInfos = Server.ServerConfig.ApiDb.
                Query<DeviceInfo>("SELECT a.Code, a.Ip, a.`Port`, a.ScriptId, b.* FROM `device_library` a JOIN `npc_proxy_link` b ON " +
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
        }

        public void UpdateConfig()
        {
            var deviceInfos = Server.ServerConfig.ApiDb.
                Query<DeviceInfo>("SELECT b.DeviceId, a.ScriptId FROM `device_library` a JOIN `npc_proxy_link` b ON " +
                                  "a.Id = b.DeviceId WHERE a.MarkedDelete = 0 AND b.ServerId = @ServerId;", new
                                  {
                                      Server.ServerConfig.ServerId
                                  });
            foreach (var deviceInfo in deviceInfos)
            {
                var deviceId = deviceInfo.DeviceId;
                if (_clients.ContainsKey(deviceId))
                {
                    _clients[deviceId].Socket.UpdateInfo(deviceInfo);
                }
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
        private static void FrequencyMonitoring(object state)
        {
            if (!_isInit)
            {
                return;
            }

            var nowTime = DateTime.Now;
            var clients = _clients.Values
                .Where(x => x.DeviceInfo != null && x.DeviceInfo.Monitoring && x.DeviceInfo.Frequency > 0 && x.NextSendTime <= nowTime);
            if (clients.Any())
            {
                Parallel.ForEach(clients, client =>
                {
                    //Log.Debug(client.DeviceInfo.Ip + " Monitoring success,当前:" + nowTime.ToString("yyyy-MM-dd HH:mm:ss fff") + ",本次:" + client.NextSendTime.ToString("yyyy-MM-dd HH:mm:ss fff"));
                    client.NextSendTime = nowTime.AddMilliseconds(client.DeviceInfo.Frequency - _wc);
                    var resError = client.Socket.SendMessage(client.Socket.HeartPacketByte);
                    if (resError != Error.Success)
                    {
                        //Log.Debug(client.DeviceInfo.Ip + " Monitoring Fail   ,下次:" + client.NextSendTime.ToString("yyyy-MM-dd HH:mm:ss fff"));
                        client.NextSendTime = client.NextSendTime.AddMilliseconds(-client.DeviceInfo.Frequency + _wc);
                        return;
                    }
                    //Log.Debug(client.DeviceInfo.Ip + " Monitoring success,下次:" + client.NextSendTime.ToString("yyyy-MM-dd HH:mm:ss fff"));

                    //Log.Debug(client.DeviceInfo.Ip + " Monitoring success,发送耗时:" + (DateTime.Now - nowTime).TotalMilliseconds);
                    //Log.Debug("-------------");
                });
            }
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

        public Error SendMessage(DeviceInfo deviceInfo)
        {
            if (!_clients.ContainsKey(deviceInfo.DeviceId))
            {
                return Error.DeviceNotExist;
            }

            return _clients[deviceInfo.DeviceId].Socket.SendMessage(deviceInfo.Instruction);
        }

        public IEnumerable<DeviceErr> SendMessage(IEnumerable<DeviceInfo> devicesList)
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
                res.Add(new DeviceErr(deviceId, _clients[deviceId].Socket.SendMessage(deviceInfo.Instruction)));
            }

            return res;
        }

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
