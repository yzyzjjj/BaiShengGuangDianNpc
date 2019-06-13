﻿using Microsoft.EntityFrameworkCore.Internal;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using NpcProxyLink.Base.Helper;
using NpcProxyLink.Base.Server;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static System.String;

namespace NpcProxyLink.Base.Logic
{
    public class SocketInfo
    {
        public DeviceInfo DeviceInfo;

        //public SocketState State;
        //public DeviceState DeviceState;
        private int _maxLogCount = 5;

        private bool _sending = false;
        ///// <summary>
        ///// 当前加工流程卡号
        ///// </summary>
        //public string FlowCard { get; set; } = Empty;
        ///// <summary>
        ///// 加工时间
        ///// </summary>
        //public string ProcessTime { get; set; } = Empty;
        ///// <summary>
        ///// 剩余加工时间
        ///// </summary>
        //public string LeftTime { get; set; } = Empty;

        public string HeartPacket;
        /// <summary>
        /// 运行状态
        /// </summary>
        private int _stateDictionaryId;
        /// <summary>
        /// 加工时间
        /// </summary>
        private int _processTimeDictionaryId;
        /// <summary>
        /// 剩余加工时间
        /// </summary>
        private int _leftTimeDictionaryId;
        /// <summary>
        /// 当前加工流程卡号
        /// </summary>
        private int _flowCardDictionaryId;


        /// <summary>
        /// 尝试连接次数
        /// </summary>
        private int _tryTime = 0;
        /// <summary>
        /// socket事件次数
        /// </summary>
        private int _connectSuccessError = 0;
        /// <summary>
        /// socket异步连接次数
        /// </summary>
        private int _connectAsyncError = 0;
        /// <summary>
        /// 尝试连接
        /// </summary>
        private bool _isTrying = false;
        /// <summary>
        /// 正在发送监控报文
        /// </summary>
        public bool Monitoring = false;
        /// <summary>
        /// 正在发送检测报文
        /// </summary>
        private bool _hearting = false;

        private readonly SocketAsyncEventArgs _args = new SocketAsyncEventArgs();

        ///<summary>
        ///接受数据缓存长度
        ///</summary>
        private const int ReceiveBufferSize = 4 * 1024;

        /// <summary>
        /// 接收数据数组
        /// </summary>
        private byte[] _receiveData;

        private Socket _socket;
        public SocketInfo(DeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
            UpdateInfo(deviceInfo);
            if (IPAddress.TryParse(DeviceInfo.Ip, out var ipAddress))
            {
                _args.RemoteEndPoint = new IPEndPoint(ipAddress, DeviceInfo.Port);
                _args.UserToken = _socket;
                _args.Completed += ConnectCompleted;
                DeviceInfo.State = SocketState.Connecting;
                _isTrying = true;
                //Log.Info("start connect");
                ConnectAsync();
            }
            else
            {
                DeviceInfo.State = SocketState.Fail;
            }
        }

        public void UpdateInfo(DeviceInfo deviceInfo)
        {
            var sv = ScriptVersionHelper.Get(deviceInfo.ScriptId);
            HeartPacket = sv?.HeartPacket ?? "";
            var ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 1);
            _stateDictionaryId = ud?.DictionaryId ?? 2;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 3);
            _processTimeDictionaryId = ud?.DictionaryId ?? 286;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 4);
            _leftTimeDictionaryId = ud?.DictionaryId ?? 285;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 6);
            _flowCardDictionaryId = ud?.DictionaryId ?? 291;
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs arg)
        {
            if (arg.SocketError == SocketError.Success)
            {
                DeviceInfo.State = SocketState.Connected;
                //Log.Info("connect success");
            }
            else
            {
                if (_connectSuccessError == 0)
                {
                    Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectSuccess ERROR, ErrMsg:{2}", DeviceInfo.Ip, DeviceInfo.Port, arg.SocketError);
                }
                _connectSuccessError++;
                if (_connectSuccessError == _maxLogCount)
                {
                    _connectSuccessError = 0;
                }
                DeviceInfo.State = SocketState.Fail;
            }

            _isTrying = false;
        }

        /// <summary>
        /// 异步Connect
        /// </summary>
        private void ConnectAsync()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                //响应超时设置
                ReceiveTimeout = 1000,
                //Blocking = false
            };
            try
            {
                _socket.ConnectAsync(_args);
            }
            catch (Exception e)
            {
                if (_connectAsyncError == 0)
                {
                    Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectAsync ERROR, ErrMsg:{2}, StackTrace：{3}", DeviceInfo.Ip, DeviceInfo.Port, e.Message, e.StackTrace);
                }
                _connectAsyncError++;
                if (_connectAsyncError == _maxLogCount)
                {
                    _connectAsyncError = 0;
                }

                DeviceInfo.State = SocketState.Fail;
            }
        }

        public void CheckState()
        {
            if (DeviceInfo.State == SocketState.Connecting)
            {
                return;
            }
            if (DeviceInfo.State == SocketState.Connected && Monitoring)
            {
                _hearting = true;
                return;
            }

            if (Heart())
            {
                DeviceInfo.State = SocketState.Connected;
                return;
            }

            if (_isTrying)
            {
                return;
            }
            if (_tryTime == 0)
            {
                Log.InfoFormat("Socket Ip:{0}, Port：{1} ReConnect", DeviceInfo.Ip, DeviceInfo.Port);
            }
            _tryTime++;
            if (_tryTime == _maxLogCount)
            {
                _tryTime = 0;
            }
            _isTrying = true;
            if (DeviceInfo.State == SocketState.Connected)
            {
                _isTrying = false;
                return;
            }
            Disconnect();
            DeviceInfo.State = SocketState.Connecting;
            ConnectAsync();
        }

        private bool UpgradeState()
        {
            var instruction = HeartPacket.IsNullOrEmpty() ? "f3,2,2c,1,ff,0,ff,0,67,12" : HeartPacket;
            try
            {
                //以英文逗号分割字符串，并去掉空字符
                var chars = instruction.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //逐个字符变为16进制字节数据
                var sendData = chars.Select(x => Convert.ToByte(x, 16)).ToArray();
                _socket.Send(sendData);
                _receiveData = new byte[ReceiveBufferSize];
                _socket.Receive(_receiveData);
                var result = _receiveData.Any(x => x != 0);
                if (result)
                {
                    var rData = _receiveData.Select(t => Convert.ToString(t, 16)).Reverse();
                    var val = rData.First(x => x != "0");
                    var index = rData.IndexOf(val);
                    var lData = rData.Skip(index);
                    var data = lData.Reverse().ToArray();
                    var start = 1 + 1 + 4 + 4 + (4 * (_stateDictionaryId - 1));
                    var str = "";
                    for (var i = 0; i < 4; i++)
                    {
                        str += data[i + start];
                    }
                    str = str.Reverse();
                    var v = Convert.ToInt32(str, 16);
                    DeviceInfo.DeviceState = v == 0 ? DeviceState.Waiting : DeviceState.Processing;
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Heart()
        {
            //Log.Error("Heart");
            var instruction = HeartPacket.IsNullOrEmpty() ? "f3,2,2c,1,ff,0,ff,0,67,12" : HeartPacket;
            try
            {
                //以英文逗号分割字符串，并去掉空字符
                var chars = instruction.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //逐个字符变为16进制字节数据
                var sendData = chars.Select(x => Convert.ToByte(x, 16)).ToArray();
                if (!_sending)
                {
                    _sending = true;
                    _socket.Send(sendData);
                    _receiveData = new byte[ReceiveBufferSize];
                    _socket.Receive(_receiveData);
                    var result = _receiveData.Any(x => x != 0);
                    if (result)
                    {
                        Task.Run(() => { UpdateInfo(_receiveData); });
                    }

                    //Log.Error("Heart success");
                    _sending = false;
                    return result;
                }
                return true;
            }
            catch (Exception)
            {
                _sending = false;
                return false;
            }
        }

        private void UpdateInfo(IEnumerable<byte> receiveData)
        {
            var rData = receiveData.Select(t => Convert.ToString(t, 16)).Reverse();
            var val = rData.FirstOrDefault(x => x != "0");
            if (val == null)
            {
                return;
            }

            var index = rData.IndexOf(val);
            var lData = rData.Skip(index);
            var data = lData.Reverse().ToArray();
            var start = 1 + 1 + 4 + 4 + (4 * (_stateDictionaryId - 1));
            var str = "";
            if (data.Length >= start + 4)
            {
                for (var i = 0; i < 4; i++)
                {
                    str += data[i + start];
                }
                str = str.Reverse();
                var v = Convert.ToInt32(str, 16);
                DeviceInfo.DeviceState = v == 0 ? DeviceState.Waiting : DeviceState.Processing;
            }

            start = 1 + 1 + 4 + 4 + (4 * (_processTimeDictionaryId - 1));
            if (data.Length >= start + 4)
            {
                str = "";
                for (var i = 0; i < 4; i++)
                {
                    str += data[i + start];
                }
                str = str.Reverse();
                DeviceInfo.ProcessTime = Convert.ToInt32(str, 16).ToString();
            }

            start = 1 + 1 + 4 + 4 + (4 * (_leftTimeDictionaryId - 1));
            if (data.Length >= start + 4)
            {
                str = "";
                for (var i = 0; i < 4; i++)
                {
                    str += data[i + start];
                }
                str = str.Reverse();
                DeviceInfo.LeftTime = Convert.ToInt32(str, 16).ToString();
            }

            start = 1 + 1 + 4 + 4 + (4 * (_flowCardDictionaryId - 1));
            if (data.Length >= start + 4)
            {
                str = "";
                for (var i = 0; i < 4; i++)
                {
                    str += data[i + start];
                }
                str = str.Reverse();
                var fid = Convert.ToInt32(str, 16);

                var flowCardName =
                    ServerConfig.ApiDb.Query<string>("SELECT FlowCardName FROM `flowcard_library` WHERE Id = @Id;", new { Id = fid }).FirstOrDefault();

                if (flowCardName != null)
                {
                    DeviceInfo.FlowCard = flowCardName;
                }
            }
        }

        ///<summary>
        ///断开连接
        ///</summary>
        public void Disconnect()
        {
            if (_socket != null)
            {
                if (_socket.Connected)
                {
                    _socket.Close();
                }
                _socket.Dispose();
            }
            DeviceInfo.State = SocketState.Close;
        }

        private void SaveDate(string data, DateTime sendTime, DateTime receiveTime, bool userSend = false)
        {
            ServerConfig.DataStorageDb
                .Execute(
                    "INSERT INTO `npc_monitoring_data` (`Ip`, `Port`, `SendTime`, `ReceiveTime`, `DealTime`, `Data`, `UserSend`, `ScriptId`) VALUES (@Ip, @Port, @sendTime, @receiveTime, @dealTime, @data, @UserSend, @ScriptId);",
                    new
                    {
                        DeviceInfo.Ip,
                        DeviceInfo.Port,
                        sendTime,
                        receiveTime,
                        dealTime = (receiveTime - sendTime).TotalMilliseconds,
                        data,
                        userSend,
                        DeviceInfo.ScriptId
                    });
        }

        #region 同步发送消息

        private Error SendMessage(byte[] messageBytes)
        {
            if (!messageBytes.Any())
            {
                return Error.InstructionError;
            }

            try
            {
                if (DeviceInfo.State == SocketState.Connected)
                {
                    if (!_sending)
                    {
                        _sending = true;
                        Monitoring = true;
                        var sendTime = DateTime.Now;
                        _socket.Send(messageBytes);

                        var tryReceive = 0;
                        while (true)
                        {
                            tryReceive++;
                            if (tryReceive == 100)
                            {
                                break;
                            }

                            //Log.Debug("thing：" + tryReceive);
                            if (_socket.Available > 0)
                            {
                                _receiveData = new byte[ReceiveBufferSize];
                                var l = _socket.Receive(_receiveData);
                                //Log.Debug("something：" + _socket.Available + "," + l);
                                if (l > 0)
                                {
                                    var copyData = new byte[l];
                                    Array.Copy(_receiveData, copyData, l);
                                    var receiveTime = DateTime.Now;
                                    if (DeviceInfo.Storage)
                                    {
                                        Task.Run(() =>
                                        {
                                            var rData = copyData.Select(t => Convert.ToString(t, 16)).Reverse();
                                            var val = rData.FirstOrDefault(x => x != "0");

                                            if (val == null)
                                            {
                                                Log.DebugFormat("Ip:{0}, Length:{1}", DeviceInfo.Ip, l);
                                                Monitoring = false;
                                                return;
                                            }

                                            var index = rData.IndexOf(val);
                                            var lData = rData.Skip(index);
                                            var data = lData.Reverse().Join(",");
                                            SaveDate(data, sendTime, receiveTime);
                                            if (_hearting)
                                            {
                                                UpdateInfo(copyData);
                                                _hearting = false;
                                            }
                                        });
                                    }
                                    break;
                                }
                            }
                            Thread.Sleep(2);
                        }

                        _sending = false;
                        Monitoring = false;
                    }
                    return Error.Success;
                }
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage ERROR, ErrMsg：{2}, StackTrace：{3}", DeviceInfo.Ip, DeviceInfo.Port, e.Message, e.StackTrace);
                _sending = false;
                Monitoring = false;
                return Error.Fail;
            }
            return Error.DeviceException;
        }

        public Error SendMessage(string messageStr)
        {
            if (messageStr.IsNullOrEmpty())
            {
                return Error.InstructionError;
            }

            try
            {
                //以英文逗号分割字符串，并去掉空字符
                string[] chars = messageStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //逐个字符变为16进制字节数据
                var sendData = chars.Select(x => Convert.ToByte(x, 16)).ToArray();
                return SendMessage(sendData);
            }
            catch (Exception)
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage InstructionError ERROR, Instruction：{2}", DeviceInfo.Ip, DeviceInfo.Port, messageStr);
                return Error.InstructionError;
            }
        }

        private string SendMessageBack(byte[] messageBytes)
        {
            var data = Empty;
            if (!messageBytes.Any())
            {
                return data;
            }

            try
            {
                if (DeviceInfo.State == SocketState.Connected)
                {
                    var sendTime = DateTime.Now;
                    _socket.Send(messageBytes);
                    _receiveData = new byte[ReceiveBufferSize];
                    _socket.Receive(_receiveData);
                    //逐字节变为16进制字符，以英文逗号隔开
                    if (_receiveData.Any(x => x > 0))
                    {
                        var rData = _receiveData.Select(t => Convert.ToString(t, 16)).Reverse();
                        var val = rData.First(x => x != "0");
                        var index = rData.IndexOf(val);
                        var lData = rData.Skip(index);
                        data = lData.Reverse().Join(",");
                    }
                    var receiveTime = DateTime.Now;

                    if (DeviceInfo.Storage)
                    {
                        Task.Run(() =>
                        {
                            SaveDate(data, sendTime, receiveTime, true);
                        });
                    }
                    return data;
                }
                return "Socket Close";
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage ERROR, ErrMsg：{2}, StackTrace：{3}", DeviceInfo.Ip, DeviceInfo.Port, e.Message, e.StackTrace);
                return e.Message;
            }
        }

        public string SendMessageBack(string messageStr)
        {
            var data = "InstructionError";
            if (messageStr.IsNullOrEmpty())
            {
                return data;
            }

            try
            {
                //以英文逗号分割字符串，并去掉空字符
                string[] chars = messageStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //逐个字符变为16进制字节数据
                var sendData = chars.Select(x => Convert.ToByte(x, 16)).ToArray();
                return SendMessageBack(sendData);
            }
            catch (Exception)
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage InstructionError ERROR, Instruction：{2}", DeviceInfo.Ip, DeviceInfo.Port, messageStr);
                return data;
            }
        }

        #endregion
    }
}
