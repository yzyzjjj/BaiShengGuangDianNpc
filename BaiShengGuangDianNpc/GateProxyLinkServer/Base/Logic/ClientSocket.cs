using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Models.Device;
using ModelBase.Models.Socket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GateProxyLinkServer.Base.Logic
{
    public class ClientSocket
    {
        public int ServerId;
        public Socket Socket;
        private CancellationTokenSource _cancellationToken;
        private static readonly int ReceiveBufferSize = 1024;
        private byte[] _mBuffer = new byte[ReceiveBufferSize];
        private SocketBufferReader _reader;
        public SocketState SocketState;
        public List<DeviceInfo> DeviceInfos = new List<DeviceInfo>();
        private readonly object _lockObj = new object();
        public DateTime LastReceiveTime;

        public ClientSocket()
        {
            LastReceiveTime = DateTime.Now;
            _cancellationToken = new CancellationTokenSource();
            SocketState = SocketState.Connected;
        }

        public void Init()
        {
            var log = $"客户端{Socket.RemoteEndPoint}成功连接";
            Log.Info(log);
            Console.WriteLine(log);

            Task.Run(() =>
            {
                ReceiveData(Socket);
            }, _cancellationToken.Token);
        }

        private void ReceiveData(Socket clientSocket)
        {
            if (SocketState == SocketState.Connected)
            {
                clientSocket.BeginReceive(_mBuffer, 0, _mBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var rEnd = Socket.EndReceive(ar);
                if (rEnd > 0)
                {
                    LastReceiveTime = DateTime.Now;
                    var partBytes = new byte[rEnd];
                    Array.Copy(_mBuffer, 0, partBytes, 0, rEnd);
                    //在此次可以对data进行按需处理

                    var b = partBytes.Take(2).ToArray();
                    var mHeader = Encoding.UTF8.GetString(b);
                    if (mHeader == SocketBufferReader.Header)
                    {
                        _reader = new SocketBufferReader();
                        partBytes = partBytes.Skip(2).ToArray();
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
                                NpcSocketMsg responseMsg = null;
                                switch (msg.MsgType)
                                {
                                    case NpcSocketMsgType.Heart:
                                        responseMsg = new NpcSocketMsg
                                        {
                                            MsgType = NpcSocketMsgType.HeartSuccess,
                                        };
                                        break;
                                    case NpcSocketMsgType.HeartSuccess:
                                        //Console.WriteLine("连接正常");
                                        break;
                                    case NpcSocketMsgType.List:
                                        DeviceInfos = JsonConvert.DeserializeObject<List<DeviceInfo>>(msg.Body);
                                        if (DeviceInfos.Any())
                                        {
                                            ServerId = DeviceInfos.First().ServerId;
                                        }
                                        break;
                                    case NpcSocketMsgType.Add:
                                    case NpcSocketMsgType.Delete:
                                    case NpcSocketMsgType.Storage:
                                    case NpcSocketMsgType.Frequency:
                                    case NpcSocketMsgType.SendBack:
                                        ServerManager.NpcSocketMsgs.Add(msg.Guid, msg);
                                        break;
                                    default:
                                        break;
                                }

                                //if (msg.MsgType != NpcSocketMsgType.Heart)
                                //{
                                //    Console.WriteLine($"{DateTime.Now} {msg.MsgType} -----------receive done");
                                //}
                                if (responseMsg != null)
                                {
                                    //Console.WriteLine($"{DateTime.Now} {msg.MsgType} -----------send done");
                                    Send(responseMsg);
                                }
                            });
                            _reader = null;
                        }

                        Socket.BeginReceive(_mBuffer, 0, _mBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                    }

                }
            }
            catch (Exception ex)
            {
                SocketState = SocketState.Close;
            }
        }

        public void Dispose()
        {
            try
            {
                var log = $"客户端{Socket.RemoteEndPoint}断开连接";
                Log.Info(log);
                Console.WriteLine(log);
                _cancellationToken.Cancel();
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                SocketState = SocketState.Close;
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public void Send(NpcSocketMsg msg)
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
                Send(System.Text.Encoding.UTF8.GetBytes(data));
            }
        }

        public void Send(byte[] byteData)
        {
            lock (_lockObj)
            {
                if (SocketState == SocketState.Connected)
                {
                    try
                    {
                        Socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), Socket);
                    }
                    catch (Exception ex)
                    {
                        SocketState = SocketState.Fail;
                    }
                }
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
            }
            catch (Exception ex)
            {
                SocketState = SocketState.Fail;
            }
        }
    }
}