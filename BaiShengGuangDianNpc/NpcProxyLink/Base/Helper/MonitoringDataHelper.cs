using NpcProxyLink.Base.Logic;
using NpcProxyLink.Base.Server;
using ServiceStack;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NpcProxyLink.Base.Helper
{
    public class MonitoringDataHelper
    {
        private static Timer _checkTimer = new Timer(SaveData, null, 5000, 2000);
        private static bool _insert;
#if !DEBUG
        /// <summary>
        /// 日志上限  1s采集 * 200台 * 2s间隔
        /// </summary>
        private const int LogMaxLength = 20 * 2 * 2;
#else
        /// <summary>
        /// 日志上限  1s采集 * 200台 * 2s间隔
        /// </summary>
        private const int LogMaxLength = 500;
#endif
        /// <summary>
        ///日志
        /// </summary>
        private static List<SocketMessage> _socketMessages = new List<SocketMessage>();
        private static void SaveData(object state)
        {
            if (_socketMessages.Count > LogMaxLength)
            {
                if (_insert)
                {
                    return;
                }

                _insert = true;
                var socketMessages = new List<SocketMessage>();
                socketMessages.AddRange(_socketMessages.Take(LogMaxLength));
                _socketMessages = _socketMessages.Skip(LogMaxLength).ToList();
                //Log.Debug($"出{_socketMessages.Count}： {socketMessages.Count(y => y.DeviceId == 6)}.--{socketMessages.Where(y => y.DeviceId == 6).GroupBy(x => x.SendTime.ToStr().Length)}");
                InsertSocketMessage(socketMessages);
                socketMessages.Clear();
                _insert = false;
            }
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="data">日志对象</param>
        /// <returns></returns>
        public static void Add(SocketMessage data)
        {
            //if (data.Ip == "192.168.1.44")
            //{
            //    Log.Debug("入：" + data.SendTime);
            //}
            _socketMessages.Add(data);
        }

        public static void Add(IEnumerable<SocketMessage> data)
        {
            _socketMessages.AddRange(data);
        }

        private static async void InsertSocketMessageAsync(IEnumerable<SocketMessage> socketMessages)
        {
            //await ServerConfig.DataStorageDb.ExecuteAsync(
            //   "INSERT npc_monitoring_data (`SendTime`, `ReceiveTime`, `DealTime`, `DeviceId`, `Ip`, `Port`, `Data`, `UserSend`, `ScriptId`, `ValNum`, `InNum`, `OutNum`) " +
            //   "VALUES (@SendTime, @ReceiveTime, @DealTime, @DeviceId, @Ip, @Port, @Data, @UserSend, @ScriptId, @ValNum, @InNum, @OutNum);"
            //   , socketMessages.OrderBy(x => x.SendTime));

            await ServerConfig.ApiDb.ExecuteAsync(
                "INSERT npc_monitoring_analysis (`SendTime`, `ReceiveTime`, `DealTime`, `DeviceId`, `Ip`, `Port`, `Data`, `UserSend`, `ScriptId`, `ValNum`, `InNum`, `OutNum`) " +
                "VALUES (@SendTime, @ReceiveTime, @DealTime, @DeviceId, @Ip, @Port, @Data, @UserSend, @ScriptId, @ValNum, @InNum, @OutNum);"
                , socketMessages.OrderBy(x => x.ReceiveTime));
        }
        private static void InsertSocketMessage(IEnumerable<SocketMessage> socketMessages)
        {
            //ServerConfig.DataStorageDb.ExecuteTrans(
            //   "INSERT npc_monitoring_data (`SendTime`, `ReceiveTime`, `DealTime`, `DeviceId`, `Ip`, `Port`, `Data`, `UserSend`, `ScriptId`, `ValNum`, `InNum`, `OutNum`) " +
            //   "VALUES (@SendTime, @ReceiveTime, @DealTime, @DeviceId, @Ip, @Port, @Data, @UserSend, @ScriptId, @ValNum, @InNum, @OutNum);"
            //   , socketMessages.OrderBy(x => x.SendTime));

            ServerConfig.ApiDb.ExecuteTrans(
                "INSERT npc_monitoring_analysis (`SendTime`, `ReceiveTime`, `DealTime`, `DeviceId`, `Ip`, `Port`, `Data`, `UserSend`, `ScriptId`, `ValNum`, `InNum`, `OutNum`) " +
                "VALUES (@SendTime, @ReceiveTime, @DealTime, @DeviceId, @Ip, @Port, @Data, @UserSend, @ScriptId, @ValNum, @InNum, @OutNum);"
                , socketMessages.OrderBy(x => x.ReceiveTime));
        }
    }
}
