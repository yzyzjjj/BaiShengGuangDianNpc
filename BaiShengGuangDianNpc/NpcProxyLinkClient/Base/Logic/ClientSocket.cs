using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using ModelBase.Models.Device;
using ModelBase.Models.Result;
using ModelBase.Models.Socket;
using Newtonsoft.Json;
using NpcProxyLinkClient.Base.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NpcProxyLinkClient.Base.Logic
{
    public class ClientSocket : IDisposable
    {
        public int ServerId;
        private Socket _socket;
        private CancellationTokenSource _cancellationToken;
        private static readonly int bufferSize = 1024 * 4;
        private byte[] _mBuffer = new byte[bufferSize];
        private SocketBufferReader _reader;
        private IPAddress _ip;
        private int _port;
        public SocketState SocketState;
        private List<DeviceInfo> _deviceInfos = new List<DeviceInfo>();

        private Timer _heartTimer;
        private readonly int _maxConnectLogCount = 5;
        private int _connectLogCount = 0;

        private bool _isSocketTry = false;
        private DateTime _tryTime;
        private readonly object _lockObj = new object();
        private bool _isNormal = false;
        private DateTime _lastTime;
        public void Init(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
            Connect();
            _heartTimer = new Timer(Heart, null, 10000, 5000);
        }

        private void Connect()
        {
            if (_connectLogCount < _maxConnectLogCount)
            {
                var msg = DateTime.Now + " Connect";
                Console.WriteLine(msg);
                Log.Error(msg);
            }

            try
            {
                Dispose();
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    SendTimeout = 2000,
                    ReceiveTimeout = 2000,
                    ReceiveBufferSize = bufferSize,
                    SendBufferSize = bufferSize
                };
                _socket.BeginConnect(_ip, _port, new AsyncCallback(ConnectCallback), _socket);
                _isSocketTry = true;
                _tryTime = DateTime.Now;
                _lastTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                _isSocketTry = false;
                SocketState = SocketState.Fail;
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket)ar.AsyncState;
                handler.EndConnect(ar);
                SocketState = SocketState.Connected;
                _cancellationToken = new CancellationTokenSource();

                var thread = new Thread(ReceiveData)
                {
                    IsBackground = true
                };
                thread.Start();
                //ReceiveData();
                //Task.Run(() =>
                //{
                //}, _cancellationToken.Token);
                var msg = $"连接Gate成功,{ServerConfig.GateIp},{ServerConfig.GatePort}";
                Log.Error(msg);
                Console.WriteLine(msg);
                _connectLogCount = 0;
            }
            catch (SocketException ex)
            {
                _isNormal = false;
                SocketState = SocketState.Fail;
                if (_connectLogCount < _maxConnectLogCount)
                {
                    var msg = $"连接Gate失败,{ServerConfig.GateIp},{ServerConfig.GatePort}";
                    Console.WriteLine(msg);
                    Log.Error(msg);
                    _connectLogCount++;
                }
            }

            _isSocketTry = false;
        }

        private void Heart(object state)
        {
            if (_isSocketTry)
            {
                if ((DateTime.Now - _lastTime).TotalSeconds > 30)
                {
                    _isSocketTry = false;
                }
                return;
            }

            if (SocketState == SocketState.Connected)
            {
                if (_lastTime != default(DateTime) && (DateTime.Now - _lastTime).TotalSeconds > 10)
                {
                    Console.WriteLine("超时");
                    SocketState = SocketState.Close;
                    return;
                }
                try
                {
                    var msg = new NpcSocketMsg
                    {
                        MsgType = NpcSocketMsgType.Heart,
                    };
                    var writer = new SocketBufferWriter();
                    writer.WriteString(msg);
                    var s = writer.Finish();
                    Send(s);
                }
                catch (Exception ex)
                {
                    _isNormal = false;
                    SocketState = SocketState.Fail;
                    Log.Error($"Heart Error, {ex.Message}, {ex.StackTrace}");
                }
            }
            else
            {
                Connect();
            }
        }

        private void ReceiveData()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (SocketState == SocketState.Connected)
            {
                try
                {
                    _socket.BeginReceive(_mBuffer, 0, _mBuffer.Length, 0, ReceiveCallback, _socket);
                }
                catch (Exception ex)
                {
                    _isNormal = false;
                    SocketState = SocketState.Fail;
                    Log.Error($"ReceiveData Error, {ex.Message}, {ex.StackTrace}");
                }
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                _lastTime = DateTime.Now;
                var ts = (Socket)ar.AsyncState;
                var rEnd = ts.EndReceive(ar);
                if (rEnd > 0)
                {
                    var partBytes = new byte[rEnd];
                    Array.Copy(_mBuffer, 0, partBytes, 0, rEnd);
                    //在此次可以对data进行按需处理

                    var mHeader = Encoding.UTF8.GetString(partBytes.Take(2).ToArray());
                    if (mHeader == SocketBufferReader.Header)
                    {
                        _reader = new SocketBufferReader();
                        partBytes = partBytes.Skip(2).ToArray();
                        _reader.MDataLength = BitConverter.ToInt32(partBytes.Take(4).ToArray(), 0);
                        partBytes = partBytes.Skip(4).ToArray();
                    }

                    if (_reader != null)
                    {
                        _reader.SocketBufferReaderAdd(partBytes);
                        if (_reader.IsValid)
                        {
                            var data = _reader.ReadString();
                            Task.Run(() =>
                            {
                                var msg = JsonConvert.DeserializeObject<NpcSocketMsg>(data);
                                //if (msg.MsgType != NpcSocketMsgType.HeartSuccess)
                                //{
                                //    Console.WriteLine($"{DateTime.Now} {msg.MsgType} -----------receive done");
                                //}
                                NpcSocketMsg responseMsg = null;
                                List<DeviceInfo> devicesList;
                                var dataErrResult = new DataErrResult();
                                switch (msg.MsgType)
                                {
                                    case NpcSocketMsgType.Heart:
                                        //responseMsg = new NpcSocketMsg
                                        //{
                                        //    MsgType = NpcSocketMsgType.HeartSuccess,
                                        //};
                                        break;
                                    case NpcSocketMsgType.HeartSuccess:
                                        //Console.WriteLine("连接正常");
                                        if (_isNormal == false)
                                        {
                                            _isNormal = true;
                                            Log.Info("连接Gate正常");
                                        }
                                        break;
                                    case NpcSocketMsgType.List:
                                        //设备列表
                                        responseMsg = new NpcSocketMsg
                                        {
                                            MsgType = msg.MsgType,
                                            Body = ClientManager.GetDevices().ToJSON()
                                        };
                                        break;
                                    case NpcSocketMsgType.Add:
                                        devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        dataErrResult.datas.AddRange(ClientManager.AddClient(devicesList));
                                        responseMsg = new NpcSocketMsg
                                        {
                                            Guid = msg.Guid,
                                            MsgType = msg.MsgType,
                                            Body = dataErrResult.ToJSON()
                                        };
                                        break;
                                    case NpcSocketMsgType.Delete:
                                        devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        dataErrResult.datas.AddRange(ClientManager.DelClient(devicesList));
                                        responseMsg = new NpcSocketMsg
                                        {
                                            Guid = msg.Guid,
                                            MsgType = msg.MsgType,
                                            Body = dataErrResult.ToJSON()
                                        };
                                        break;
                                    case NpcSocketMsgType.Update:
                                        devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        dataErrResult.datas.AddRange(ClientManager.UpdateClient(devicesList));
                                        responseMsg = new NpcSocketMsg
                                        {
                                            Guid = msg.Guid,
                                            MsgType = msg.MsgType,
                                            Body = dataErrResult.ToJSON()
                                        };
                                        break;
                                    case NpcSocketMsgType.Storage:
                                        devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        dataErrResult.datas.AddRange(ClientManager.SetStorage(devicesList));
                                        responseMsg = new NpcSocketMsg
                                        {
                                            Guid = msg.Guid,
                                            MsgType = msg.MsgType,
                                            Body = dataErrResult.ToJSON()
                                        };
                                        break;
                                    case NpcSocketMsgType.Frequency:
                                        devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        dataErrResult.datas.AddRange(ClientManager.SetFrequency(devicesList));
                                        responseMsg = new NpcSocketMsg
                                        {
                                            Guid = msg.Guid,
                                            MsgType = msg.MsgType,
                                            Body = dataErrResult.ToJSON()
                                        };
                                        break;
                                    case NpcSocketMsgType.SendBack:
                                        devicesList = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        var messageResult = new MessageResult();
                                        messageResult.messages.AddRange(ClientManager.SendMessageBack(devicesList));
                                        responseMsg = new NpcSocketMsg
                                        {
                                            Guid = msg.Guid,
                                            MsgType = msg.MsgType,
                                            Body = messageResult.ToJSON()
                                        };
                                        break;
                                    default:
                                        break;
                                }
                                if (responseMsg != null)
                                {
                                    //Console.WriteLine($"{DateTime.Now} {msg.MsgType} -----------send done");
                                    Send(responseMsg);
                                }

                            });
                            _reader = null;
                        }

                        ReceiveData();
                        //_socket.BeginReceive(_mBuffer, 0, _mBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ReceiveCallback Error, {ex.Message}, {ex.StackTrace}");
                _isNormal = false;
                SocketState = SocketState.Close;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_socket != null && _socket.Connected)
                {
                    _cancellationToken.Cancel();
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
                SocketState = SocketState.Close;
            }
            catch (Exception ex)
            {
                Log.Error($"Dispose Error, {ex.Message}, {ex.StackTrace}");
                // ignored
            }
        }

        private void Send(NpcSocketMsg msg)
        {
            if (SocketState == SocketState.Connected)
            {
                var writer = new SocketBufferWriter();
                writer.WriteString(msg);
                Send(writer.Finish());
            }
        }

        public void Send(string data)
        {
            if (SocketState == SocketState.Connected)
            {
                Send(Encoding.UTF8.GetBytes(data));
            }
        }

        private void Send(byte[] byteData)
        {
            lock (_lockObj)
            {
                if (SocketState == SocketState.Connected)
                {
                    try
                    {
                        _socket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, _socket);
                    }
                    catch (Exception ex)
                    {
                        _isNormal = false;
                        SocketState = SocketState.Fail;
                        Log.Error($"Send Error, {ex.Message}, {ex.StackTrace}");
                    }
                }
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var ts = (Socket)ar.AsyncState;
                ts.EndSend(ar);
            }
            catch (Exception ex)
            {
                _isNormal = false;
                SocketState = SocketState.Fail;
                Log.Error($"SendCallback Error, {ex.Message}, {ex.StackTrace}");
            }
        }
    }
}