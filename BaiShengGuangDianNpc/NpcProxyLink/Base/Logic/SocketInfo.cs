﻿using Microsoft.EntityFrameworkCore.Internal;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using NpcProxyLink.Base.Helper;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NpcProxyLink.Base.Logic
{
    public class SocketInfo
    {
        public string Ip;
        public int Port;
        public bool Storage;
        public SocketState State;
        public DeviceState DeviceState;
        public string HeartPacket;
        public int DictionaryId;

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
            Ip = deviceInfo.Ip;
            Port = deviceInfo.Port;
            Storage = deviceInfo.Storage;
            UpdateInfo(deviceInfo);
            if (IPAddress.TryParse(Ip, out var ipAddress))
            {
                _args.RemoteEndPoint = new IPEndPoint(ipAddress, Port);
                _args.UserToken = _socket;
                _args.Completed += ConnectCompleted;
                State = SocketState.Connecting;
                ConnectAsync();
            }
            else
            {
                State = SocketState.Fail;
            }
        }

        public void UpdateInfo(DeviceInfo deviceInfo)
        {
            var sv = ScriptVersionHelper.Get(deviceInfo.ScriptId);
            HeartPacket = sv?.HeartPacket ?? "";
            var ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 1);
            DictionaryId = ud?.DictionaryId ?? 2;
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs arg)
        {
            if (arg.SocketError == SocketError.Success)
            {
                State = SocketState.Connected;
            }
            else
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectSuccess ERROR, ErrMsg:{2}", Ip, Port, arg.SocketError);
                State = SocketState.Fail;
            }
        }


        /// <summary>
        /// 异步Connect
        /// </summary>
        public void ConnectAsync()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                //响应超时设置
                ReceiveTimeout = 500,
                Blocking = true
            };
            try
            {
                _socket.ConnectAsync(_args);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectAsync ERROR, ErrMsg:{2}", Ip, Port, e.Message);
                State = SocketState.Fail;
            }
        }

        public void CheckState()
        {
            if (State == SocketState.Connecting)
            {
                return;
            }
            if (Heart())
            {
                State = SocketState.Connected;
                return;
            }

            Log.InfoFormat("Socket Ip:{0}, Port：{1} ReConnect", Ip, Port);

            Disconnect();
            State = SocketState.Connecting;
            ConnectAsync();
        }

        public bool Heart()
        {

            var instruction = HeartPacket.IsNullOrEmpty() ? "0xF3,0x02,0x2C,0x01,0xFF,0x00,0xFF,0x00,0x67,0x12" : HeartPacket;
            try
            {
                //以英文逗号分割字符串，并去掉空字符
                var chars = instruction.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //逐个字符变为16进制字节数据
                var sendData = chars.Select(x => Convert.ToByte(x, 16)).ToArray();
                try
                {
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
                        var start = 1 + 1 + 4 + 4 + (4 * (DictionaryId - 1));
                        var str = "";
                        for (var i = 0; i < 4; i++)
                        {
                            str += data[i + start];
                        }
                        str = str.Reverse();
                        var v = Convert.ToInt32(str, 16);
                        DeviceState = v == 0 ? DeviceState.Waiting : DeviceState.Processing;
                    }
                    return result;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
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
            State = SocketState.Close;
        }

        public void SaveDate(string data, DateTime sendTime, DateTime receiveTime, bool userSend = false)
        {
            Server.ServerConfig.DataStoragDb
                .Execute(
                    "INSERT INTO `npc_monitoring_data` (`Ip`, `Port`, `SendTime`, `ReceiveTime`, `DealTime`, `Data`, `UserSend`) VALUES (@Ip, @Port, @sendTime, @receiveTime, @dealTime, @data, @UserSend);",
                    new
                    {
                        Ip,
                        Port,
                        sendTime,
                        receiveTime,
                        dealTime = (receiveTime - sendTime).TotalMilliseconds,
                        data,
                        userSend
                    });

        }

        #region 同步发送消息

        public Error SendMessage(byte[] messageBytes)
        {
            if (!messageBytes.Any())
            {
                return Error.InstructionError;
            }

            try
            {
                if (State == SocketState.Connected)
                {
                    var sendTime = DateTime.Now;
                    _socket.Send(messageBytes);
                    _receiveData = new byte[ReceiveBufferSize];
                    _socket.Receive(_receiveData);
                    var receiveTime = DateTime.Now;
                    if (Storage)
                    {
                        Task.Run(() =>
                        {
                            var rData = _receiveData.Select(t => Convert.ToString(t, 16)).Reverse();
                            var val = rData.First(x => x != "0");
                            var index = rData.IndexOf(val);
                            var lData = rData.Skip(index);
                            var data = lData.Reverse().Join(",");
                            SaveDate(data, sendTime, receiveTime);
                        });
                    }
                    return Error.Success;
                }
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage ERROR, ErrMsg：{2}", Ip, Port, e.Message);
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
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage InstructionError ERROR, Instruction：{2}", Ip, Port, messageStr);
                return Error.InstructionError;
            }
        }

        public string SendMessageBack(byte[] messageBytes)
        {
            var data = string.Empty;
            if (!messageBytes.Any())
            {
                return data;
            }

            try
            {
                if (State == SocketState.Connected)
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

                    if (Storage)
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
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage ERROR, ErrMsg：{2}", Ip, Port, e.Message);
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
                Log.ErrorFormat("Socket Ip:{0}, Port：{1} SendMessage InstructionError ERROR, Instruction：{2}", Ip, Port, messageStr);
                return data;
            }
        }

        #endregion
    }
}
