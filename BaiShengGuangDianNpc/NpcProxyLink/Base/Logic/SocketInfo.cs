using Microsoft.EntityFrameworkCore.Internal;
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
using System.Threading.Tasks;
using static System.String;

namespace NpcProxyLink.Base.Logic
{
    public class SocketInfo
    {
        public string Ip;
        public int Port;
        public bool Storage;
        public SocketState State;
        public DeviceState DeviceState;
        /// <summary>
        /// 当前加工流程卡号
        /// </summary>
        public string FlowCard { get; set; } = Empty;
        /// <summary>
        /// 加工时间
        /// </summary>
        public string ProcessTime { get; set; } = Empty;
        /// <summary>
        /// 剩余加工时间
        /// </summary>
        public string LeftTime { get; set; } = Empty;

        public string HeartPacket;
        private bool initFlag = false;
        /// <summary>
        /// 运行状态
        /// </summary>
        public int StateDictionaryId;
        /// <summary>
        /// 加工时间
        /// </summary>
        public int ProcessTimeDictionaryId;
        /// <summary>
        /// 剩余加工时间
        /// </summary>
        public int LeftTimeDictionaryId;
        /// <summary>
        /// 当前加工流程卡号
        /// </summary>
        public int FlowCardDictionaryId;


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
        private bool isTrying = false;

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
                isTrying = true;
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
            StateDictionaryId = ud?.DictionaryId ?? 2;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 3);
            ProcessTimeDictionaryId = ud?.DictionaryId ?? 286;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 4);
            LeftTimeDictionaryId = ud?.DictionaryId ?? 285;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 6);
            FlowCardDictionaryId = ud?.DictionaryId ?? 291;
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs arg)
        {
            if (arg.SocketError == SocketError.Success)
            {
                State = SocketState.Connected;
            }
            else
            {
                if (_connectSuccessError == 0)
                {
                    Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectSuccess ERROR, ErrMsg:{2}", Ip, Port, arg.SocketError);
                }
                _connectSuccessError++;
                if (_connectSuccessError == 5)
                {
                    _connectSuccessError = 0;
                }
                State = SocketState.Fail;
            }

            isTrying = false;
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
                if (_connectAsyncError == 0)
                {
                    Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectAsync ERROR, ErrMsg:{2}", Ip, Port, e.Message);
                }
                _connectAsyncError++;
                if (_connectAsyncError == 5)
                {
                    _connectAsyncError = 0;
                }

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

            if (isTrying)
            {
                return;
            }
            if (_tryTime == 0)
            {
                Log.InfoFormat("Socket Ip:{0}, Port：{1} ReConnect", Ip, Port);
            }
            _tryTime++;
            if (_tryTime == 5)
            {
                _tryTime = 0;
            }
            isTrying = true;
            Disconnect();
            State = SocketState.Connecting;
            ConnectAsync();
        }

        private bool UpgradeState()
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
                        var start = 1 + 1 + 4 + 4 + (4 * (StateDictionaryId - 1));
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

        private bool Heart()
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
                        Task.Run(() =>
                        {
                            var rData = _receiveData.Select(t => Convert.ToString(t, 16)).Reverse();
                            var val = rData.First(x => x != "0");
                            var index = rData.IndexOf(val);
                            var lData = rData.Skip(index);
                            var data = lData.Reverse().ToArray();
                            var start = 1 + 1 + 4 + 4 + (4 * (StateDictionaryId - 1));
                            var str = "";
                            if (data.Length >= start + 4)
                            {
                                for (var i = 0; i < 4; i++)
                                {
                                    str += data[i + start];
                                }
                                str = str.Reverse();
                                var v = Convert.ToInt32(str, 16);
                                DeviceState = v == 0 ? DeviceState.Waiting : DeviceState.Processing;
                            }

                            start = 1 + 1 + 4 + 4 + (4 * (ProcessTimeDictionaryId - 1));
                            if (data.Length >= start + 4)
                            {
                                str = "";
                                for (var i = 0; i < 4; i++)
                                {
                                    str += data[i + start];
                                }
                                str = str.Reverse();
                                ProcessTime = Convert.ToInt32(str, 16).ToString();
                            }

                            start = 1 + 1 + 4 + 4 + (4 * (LeftTimeDictionaryId - 1));
                            if (data.Length >= start + 4)
                            {
                                str = "";
                                for (var i = 0; i < 4; i++)
                                {
                                    str += data[i + start];
                                }
                                str = str.Reverse();
                                LeftTime = Convert.ToInt32(str, 16).ToString();
                            }

                            start = 1 + 1 + 4 + 4 + (4 * (FlowCardDictionaryId - 1));
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
                                    FlowCard = flowCardName;
                                }
                            }

                        });
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
            Server.ServerConfig.DataStorageDb
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
            var data = Empty;
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
