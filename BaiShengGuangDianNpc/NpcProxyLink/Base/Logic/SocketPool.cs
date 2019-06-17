//using ModelBase.Base.EnumConfig;
//using ModelBase.Base.Logger;
//using ModelBase.Models.Device;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;

//namespace NpcProxyLink.Base.Logic
//{
//    public class SocketPool
//    {
//        private int _socketCount = 2;

//        private List<MySocket> sockets = new List<MySocket>();
//        public void Init(IPAddress ip, int port)
//        {
//            for (int i = 0; i < _socketCount; i++)
//            {

//                sockets.Add(socket);
//            }
//        }

//    }

//    public class MySocket
//    {
        
//        public SocketState State;
//        public Socket Socket;
//        private IPAddress Ip;
//        private int  Port;
//        /// <summary>
//        /// 尝试连接
//        /// </summary>
//        private bool _isTrying = false;

//        private int _maxLogCount = 20;
//        /// <summary>
//        /// socket事件次数
//        /// </summary>
//        private int _connectSuccessError = 0;
//        /// <summary>
//        /// socket异步连接次数
//        /// </summary>
//        private int _connectAsyncError = 0;

//        private readonly SocketAsyncEventArgs _args;

//        public MySocket(IPAddress ip, int port)
//        {
//            Ip = ip;
//            Port = port;
//            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
//            {
//                //响应超时设置
//                ReceiveTimeout = 1000,
//                //Blocking = false
//            };
//            _args = new SocketAsyncEventArgs
//            {
//                RemoteEndPoint = new IPEndPoint(Ip, Port),
//                UserToken = Socket
//            };
//            _args.Completed += ConnectCompleted;
//        }


//        private void ConnectAsync()
//        {
//            try
//            {
//                Socket.ConnectAsync(_args);
//            }
//            catch (Exception e)
//            {
//                if (_connectAsyncError == 0)
//                {
//                    Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectAsync ERROR, ErrMsg:{2}, StackTrace：{3}", Ip.ToString(), Port, e.Message, e.StackTrace);
//                }
//                _connectAsyncError++;
//                if (_connectAsyncError == _maxLogCount)
//                {
//                    _connectAsyncError = 0;
//                }

//                State = SocketState.Fail;
//            }
//        }
//        private void ConnectCompleted(object sender, SocketAsyncEventArgs arg)
//        {
//            if (arg.SocketError == SocketError.Success)
//            {
//                State = SocketState.Connected;
//            }
//            else
//            {
//                if (_connectSuccessError == 0)
//                {
//                    Log.ErrorFormat("Socket Ip:{0}, Port：{1} ConnectSuccess ERROR, ErrMsg:{2}", Ip.ToString(), Port, arg.SocketError);
//                }
//                _connectSuccessError++;
//                if (_connectSuccessError == _maxLogCount)
//                {
//                    _connectSuccessError = 0;
//                }
//                State = SocketState.Fail;
//            }

//            _isTrying = false;
//        }
//    }
//}
