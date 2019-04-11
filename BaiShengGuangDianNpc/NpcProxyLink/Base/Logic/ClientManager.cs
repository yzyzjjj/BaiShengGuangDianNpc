using ModelBase.Base.Logger;
using ModelBase.Base.Logic;
using ModelBase.Models.Device;
using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ModelBase.Base.EnumConfig;

namespace NpcProxyLink.Base.Logic
{
    public class ClientManager : IManager
    {
        // deviceId, client
        private static ConcurrentDictionary<int, Client> _clients = new ConcurrentDictionary<int, Client>();
        private static Timer _frequencyTimer = new Timer(FrequencyMonitoring, null, 5000, 10);

        private static Timer _checkTimer = new Timer(CheckClientState, null, 5000, 10000);
        private static bool _isInit;
        public void LoadConfig()
        {
            _clients.Clear();
            var deviceInfos = Server.ServerConfig.DeviceDb.
                Query<DeviceInfo>("SELECT a.Ip, a.`Port`, a.ScriptId, b.* FROM `device_library` a JOIN `npc_proxy_link` b ON " +
                                  "a.Id = b.DeviceId WHERE a.MarkedDelete = 0 AND b.ServerId = @ServerId;", new
                                  {
                                      Server.ServerConfig.ServerId
                                  });
            foreach (var deviceInfo in deviceInfos)
            {
                if (!AddClient(deviceInfo))
                {
                    Log.ErrorFormat("ClientManager AddClient Fail,{0},{1}:{2}",
                        deviceInfo.DeviceId, deviceInfo.Ip, deviceInfo.Port);
                }
            }

            _isInit = true;
        }

        public void UpdateConfig()
        {
            var deviceInfos = Server.ServerConfig.DeviceDb.
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
                c.Socket.CheckState();
                c.DeviceInfo.State = c.Socket.State;
                c.DeviceInfo.DeviceState = c.Socket.DeviceState;
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

            var comTime = DateTime.Now;
            var clients = _clients.Values
                .Where(x => x.DeviceInfo.Monitoring && x.DeviceInfo.Frequency > 0 && !x.DeviceInfo.Instruction.IsNullOrEmpty() && x.LastSendTime <= comTime);

            Parallel.ForEach(clients, client =>
            {
                client.LastSendTime = DateTime.Now.AddMilliseconds(client.DeviceInfo.Frequency);
                client.Socket.SendMessage(client.DeviceInfo.Instruction);
            });
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
            var client = new Client
            {
                DeviceInfo = deviceInfo
            };
            if (!_clients.TryAdd(deviceInfo.DeviceId, client))
            {
                return false;
            }

            client.Init();
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
            if (_clients.TryGetValue(deviceInfo.DeviceId, out Client client))
            {
                client.DeviceInfo.Storage = deviceInfo.Storage;
                client.Socket.Storage = deviceInfo.Storage;
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
