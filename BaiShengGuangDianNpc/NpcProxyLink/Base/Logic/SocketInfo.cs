using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.EntityFrameworkCore.Internal;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using NpcProxyLink.Base.Helper;
using NpcProxyLink.Base.Server;
using ServiceStack;

namespace NpcProxyLink.Base.Logic
{
    public class SocketInfo
    {
        public DeviceInfo DeviceInfo;

        private int _maxLogCount = 20;
        private int _sendCount = 10;
        private int _maxTryReceive = 1000;
        private int _sleep = 0;
        private bool _sending = false;

        public string HeartPacket;
        private int ValNum { get; set; }
        private int InNum { get; set; }
        private int OutNum { get; set; }
        public byte[] HeartPacketByte;
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
        private int _connectCompletedError = 0;
        /// <summary>
        /// socket异步连接次数
        /// </summary>
        private int _connectAsyncError = 0;
        /// <summary>
        /// 发送超时次数
        /// </summary>
        private int _sendError = 0;
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

        /// <summary>
        /// 连接事件
        /// </summary>
        private SocketAsyncEventArgs _connectArgs;
        /// <summary>
        /// 发送事件
        /// </summary>
        private SocketAsyncEventArgs _sendArgs;
        /// <summary>
        /// 接收事件
        /// </summary>
        private SocketAsyncEventArgs _receiveArgs;

        /// <summary>
        /// 远端地址
        /// </summary>
        private IPEndPoint _endPoint;
        private int _tryReceive;
        private SocketMessage _socketMessage;
        //private SocketMsgManager _socketMsgManager = new SocketMsgManager();
        ///<summary>
        ///接受数据缓存长度
        ///</summary>
        private const int ReceiveBufferSize = 4 * 1024;

        private Socket _socket;
        public SocketInfo(DeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
            UpdateInfo(deviceInfo);
            if (IPAddress.TryParse(DeviceInfo.Ip, out var ipAddress))
            {
                _endPoint = new IPEndPoint(ipAddress, DeviceInfo.Port);
                if (_connectArgs == null)
                {
                    _connectArgs = new SocketAsyncEventArgs { RemoteEndPoint = _endPoint, UserToken = _socket };
                    _connectArgs.Completed += OnConnectedCompleted;
                }

                DeviceInfo.State = SocketState.Connecting;
                _isTrying = true;
                //Log.Debug($"Ip:{DeviceInfo.Ip} Start Connect");
                ConnectAsync();
            }
            else
            {
                DeviceInfo.State = SocketState.Fail;
            }
        }

        /// <summary>
        /// 异步Connect
        /// </summary>
        private void ConnectAsync()
        {
            //Console.WriteLine($"当前:{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, 开始重连---------");
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                //响应超时设置
                //ReceiveTimeout = 1000,
                //Blocking = true,
            };

            _socket.ConnectAsync(_connectArgs);
        }

        public void UpdateInfo(DeviceInfo deviceInfo)
        {
            DeviceInfo.Update(deviceInfo);
            var sv = ScriptVersionHelper.Get(deviceInfo.ScriptId);
            HeartPacket = sv?.HeartPacket ?? "f3,2,2c,1,ff,0,ff,0,67,12";
            //以英文逗号分割字符串，并去掉空字符,逐个字符变为16进制字节数据
            HeartPacketByte = HeartPacket.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToByte(x, 16)).ToArray();
            var heartPackets = HeartPacket.Split(",");
            ValNum = Convert.ToInt32(heartPackets[3] + heartPackets[2], 16);
            InNum = Convert.ToInt32(heartPackets[5] + heartPackets[4], 16);
            OutNum = Convert.ToInt32(heartPackets[7] + heartPackets[6], 16);
            var ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 1);
            _stateDictionaryId = ud?.DictionaryId ?? 2;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 3);
            _processTimeDictionaryId = ud?.DictionaryId ?? 286;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 4);
            _leftTimeDictionaryId = ud?.DictionaryId ?? 285;
            ud = UsuallyDictionaryHelper.Get(deviceInfo.ScriptId, 6);
            _flowCardDictionaryId = ud?.DictionaryId ?? 291;
        }

        public void CheckState()
        {
            if (_isTrying)
            {
                return;
            }
            if (DeviceInfo.State != SocketState.Connected)
            {
                DeviceInfo.DeviceState = DeviceState.UnInit;
                DeviceInfo.State = SocketState.Fail;
                if (_tryTime == 0)
                {
                    //Log.Debug("Ip:{0}, Port：{1} ReConnect", DeviceInfo.Ip, DeviceInfo.Port);
                }
                _tryTime++;
                if (_tryTime == _maxLogCount)
                {
                    _tryTime = 0;
                }
                _isTrying = true;
                Disconnect();
                DeviceInfo.State = SocketState.Connecting;
                ConnectAsync();
            }
            else
            {
                if (DeviceInfo.Monitoring && DeviceInfo.Frequency <= 2000)
                {
                    _hearting = true;
                    return;
                }

                if (Monitoring)
                {
                    _hearting = true;
                    return;
                }

                if (Heart())
                {
                    _hearting = true;
                }
            }
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
                var receiveData = new byte[ReceiveBufferSize];
                _socket.Receive(receiveData);
                var result = receiveData.Any(x => x != 0);
                if (result)
                {
                    var rData = receiveData.Select(t => Convert.ToString(t, 16)).Reverse();
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
            return SendMessageAsync(HeartPacketByte) == Error.Success;
        }

        private void UpdateStateInfo(SocketMessage socketMessage)
        {
            var data = socketMessage.DataStrList.Skip(10).ToArray();

            var start = (_stateDictionaryId - 1) * 4;
            if (data.Length >= start + 4)
            {
                var str = data.Skip(start).Take(4).Reverse().Join("");
                var v = Convert.ToInt32(str, 16);
                DeviceInfo.DeviceState = v == 0 ? DeviceState.Waiting : DeviceState.Processing;
            }

            start = (_processTimeDictionaryId - 1) * 4;
            if (data.Length >= start + 4)
            {
                var str = data.Skip(start).Take(4).Reverse().Join("");
                DeviceInfo.ProcessTime = Convert.ToInt32(str, 16).ToString();
            }

            start = (_leftTimeDictionaryId - 1) * 4;
            if (data.Length >= start + 4)
            {
                var str = data.Skip(start).Take(4).Reverse().Join("");
                DeviceInfo.LeftTime = Convert.ToInt32(str, 16).ToString();
            }

            start = (_flowCardDictionaryId - 1) * 4;
            if (data.Length >= start + 4)
            {
                var str = data.Skip(start).Take(4).Reverse().Join("");
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
                    try
                    {
                        _socket.Shutdown(SocketShutdown.Both);
                    }
                    catch (SocketException ex)
                    {
                    }
                    finally
                    {
                        _socket.Close();
                    }
                }
            }
            DeviceInfo.State = SocketState.Close;
        }

        #region 异步  心跳和监控

        /// <summary>
        /// 连接成功的事件回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectedCompleted(object sender, SocketAsyncEventArgs e)
        {
            //Console.WriteLine($"当前:{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, 重连返回======={e.SocketError}");
            if (e.SocketError == SocketError.Success)
            {
                _tryTime = 0;
                //Console.WriteLine($"当前:{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, 重连成功=======");
                DeviceInfo.State = SocketState.Connected;
                //Log.Debug($"连接服务器[{_socket.RemoteEndPoint}]成功！");
                //开启新的接受消息异步操作事件
                _receiveArgs = new SocketAsyncEventArgs { RemoteEndPoint = _endPoint };
                var receiveBuffer = new byte[ReceiveBufferSize];
                //设置消息的缓冲区大小
                _receiveArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                //绑定回调事件
                _receiveArgs.Completed += OnReceiveCompleted;
                _socket.ReceiveAsync(_receiveArgs);
            }
            _isTrying = false;
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageBytes"></param>
        /// <param name="sendTime"></param>
        public Error SendMessageAsync(byte[] messageBytes, DateTime sendTime = default(DateTime))
        {
            if (!messageBytes.Any())
            {
                return Error.InstructionError;
            }

            try
            {
                if (DeviceInfo.State == SocketState.Connected)
                {
                    if (_sending)
                    {
                        if (_stopwatch.ElapsedMilliseconds > _maxTryReceive)
                        {
                            _sending = false;
                            Monitoring = false;
                            //if (DeviceInfo.Ip == "192.168.1.83")
                            //{
                            //    ////Console.WriteLine($"当前{_stopwatch.ElapsedMilliseconds}:{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, Count：{ _socketMessage.DataList.Count}太慢了=======");
                            //    Log.Error($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, Count：{ _socketMessage.DataList.Count} 连接异常=======");
                            //}
                            _stopwatch.Reset();
                            _sendError++;
                            if (_sendError > _sendCount)
                            {
                                DeviceInfo.State = SocketState.Fail;
                                return Error.Fail;
                            }
                        }
                        else
                        {
                            return Error.ServerBusy;
                        }
                    }

                    _sending = true;
                    Monitoring = true;
                    _socketMessage = new SocketMessage
                    {
                        SendTime = sendTime == default(DateTime) ? DateTime.Now : sendTime,
                        DeviceId = DeviceInfo.DeviceId,
                        Ip = DeviceInfo.Ip,
                        Port = DeviceInfo.Port,
                        ScriptId = DeviceInfo.ScriptId,
                        ValNum = ValNum,
                        InNum = InNum,
                        OutNum = OutNum
                    };
                    if (_sendArgs == null)
                    {
                        _sendArgs = new SocketAsyncEventArgs { RemoteEndPoint = _endPoint, UserToken = _socket };
                        _sendArgs.Completed += OnSendCompleted;
                    }
                    _sendArgs.SetBuffer(messageBytes, 0, messageBytes.Length);
                    _socket.SendAsync(_sendArgs);
                    _stopwatch.Start();
                    //Console.WriteLine($"当前{_stopwatch.ElapsedMilliseconds}: {_socketMessage.SendTime:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, Count：{ _socketMessage.DataList.Count}Send=======");
                    return Error.Success;
                }
            }
            catch (Exception e)
            {
                //Log.Debug($"Ip:{DeviceInfo.Ip} SendMessage ERROR, ErrMsg：{e.Message}, StackTrace：{e.StackTrace}");
                _sending = false;
                Monitoring = false;
                DeviceInfo.State = SocketState.Fail;
                return Error.Fail;
            }
            return Error.DeviceException;
        }

        /// <summary>
        /// 发送消息回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                return;
            }

            //Console.WriteLine($"当前:{_socketMessage.SendTime:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, Count：{ _socketMessage.DataList.Count}Send=======");
        }

        /// <summary>
        /// 接受消息的回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.OperationAborted)
            {
                return;
            }

            _tryReceive++;
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                var len = e.BytesTransferred;
                var receiveBuffer = e.Buffer;
                var receiveData = new byte[len];
                Buffer.BlockCopy(receiveBuffer, 0, receiveData, 0, len);
                //Console.WriteLine($"当前:{_socketMessage.SendTime:yyyy-MM-dd HH:mm:ss fff}, Id：{DeviceInfo.DeviceId}, Ip:{ DeviceInfo.Ip}, Count：{ _socketMessage.DataList.Count}Receive-----------:{_tryReceive},{len}");
                if (_socketMessage.DataList.Count == 0 && receiveData[0] != 243)
                {
                    _sending = false;
                    Monitoring = false;
                    _tryReceive = _sendError = 0;
                    _stopwatch.Reset();
                    return;
                }

                _socketMessage.DataList.AddRange(receiveData);
                if (_socketMessage.IsAll())
                {
                    _stopwatch.Stop();
                    _socketMessage.ReceiveTime = DateTime.Now;
                    //Console.WriteLine($"当前:{_socketMessage.ReceiveTime:yyyy-MM-dd HH:mm:ss fff} Ip:{ DeviceInfo.Ip}, Port：{ DeviceInfo.Port}  Receive Done-----------:{_socketMessage.DataList.Count},{_stopwatch.ElapsedMilliseconds}");
                    if (DeviceInfo.Storage)
                    {
                        MonitoringDataHelper.Add(_socketMessage);
                    }

                    if (_hearting)
                    {
                        UpdateStateInfo(_socketMessage);
                        _hearting = false;
                    }
                    _sending = false;
                    Monitoring = false;
                    _tryReceive = _sendError = 0;
                    _stopwatch.Reset();
                    //Console.WriteLine($"当前:{_stopwatch.ElapsedMilliseconds}");
                }
                _socket.ReceiveAsync(_receiveArgs);
            }
            else if (e.SocketError == SocketError.ConnectionReset && e.BytesTransferred == 0)
            {
                DeviceInfo.State = SocketState.Connecting;
                ConnectAsync();
            }
            else
            {
                return;
            }
        }
        #endregion

        #region 同步 控制
        private string SendMessageBack(byte[] messageBytes)
        {
            var data = string.Empty;
            if (!messageBytes.Any())
            {
                return data;
            }

            if (DeviceInfo.State != SocketState.Connected)
            {
                return "连接异常";
            }

            var backSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                //响应超时设置
                //SendTimeout = 500,
                //ReceiveTimeout = 500,
            };
            try
            {
                backSocket.Connect(DeviceInfo.Ip, DeviceInfo.Port);
                var socketMessage = new SocketMessage
                {
                    SendTime = DateTime.Now,
                    UserSend = true,
                    DeviceId = DeviceInfo.DeviceId,
                    Ip = DeviceInfo.Ip,
                    Port = DeviceInfo.Port,
                    ScriptId = DeviceInfo.ScriptId,
                    ValNum = ValNum,
                    InNum = InNum,
                    OutNum = OutNum
                };
                var sw = new Stopwatch();
                backSocket.Send(messageBytes);
                sw.Start();
                var bTryReceive = 0;
                while (true)
                {
                    bTryReceive++;
                    var len = backSocket.Available;
                    if (len > 0)
                    {
                        var receiveData = new byte[len];
                        backSocket.Receive(receiveData);
                        if (socketMessage.DataList.Count == 0)
                        {
                            if (receiveData[0] != 243)
                            {
                                continue;
                            }
                        }
                        socketMessage.DataList.AddRange(receiveData);
                        if (socketMessage.IsAll())
                        {
                            sw.Stop();
                            socketMessage.ReceiveTime = DateTime.Now;
                            break;
                        }
                    }
                    if (sw.ElapsedMilliseconds >= _maxTryReceive)
                    {
                        sw.Stop();
                        break;
                    }
                    Thread.Sleep(_sleep);
                }
                backSocket.Shutdown(SocketShutdown.Both);
                backSocket.Close();

                if (sw.ElapsedMilliseconds < _maxTryReceive)
                {
                    if (DeviceInfo.Storage)
                    {
                        MonitoringDataHelper.Add(socketMessage);
                    }
                    return socketMessage.Data;
                }
                return "没数据返回";
            }
            catch (Exception e)
            {
                Log.Error($"Ip:{DeviceInfo.Ip} SendMessage ERROR, ErrMsg：{e.Message}, StackTrace：{e.StackTrace}");
                return "发送错误," + e.Message;
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
                //以英文逗号分割字符串，并去掉空字符,逐个字符变为16进制字节数据
                var sendData = messageStr.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToByte(x, 16)).ToArray();
                return SendMessageBack(sendData);
            }
            catch (Exception)
            {
                Log.Debug($"Ip:{DeviceInfo.Ip} SendMessage InstructionError ERROR, Instruction：{messageStr}");
                return data;
            }
        }

        #endregion
    }
}
