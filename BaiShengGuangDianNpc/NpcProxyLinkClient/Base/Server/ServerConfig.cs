using Microsoft.Extensions.Configuration;
using ModelBase.Base.Dapper;
using ModelBase.Base.EnumConfig;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using ModelBase.Models.Socket;
using Newtonsoft.Json;
using NpcProxyLinkClient.Base.Helper;
using NpcProxyLinkClient.Base.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ModelBase.Models.Server;

namespace NpcProxyLinkClient.Base.Server
{
    public class ServerConfig
    {
        public static DataBase ApiDb;
        public static DataBase DataWriteDb;
        public static ClientManager ClientManager;
        public static int ServerId;
        public static RedisCacheHelper RedisHelper;
        private static Dictionary<string, Action> Loads;
        public static string ServerIp;
        public static int ServerPort;
        public static string GateIp;
        public static int GatePort;
        public static void Init(IConfiguration configuration)
        {
            ApiDb = new DataBase(configuration.GetConnectionString("ApiDb"));
            ServerId = configuration.GetAppSettings<int>("ServerId");
            ServerIp = configuration.GetAppSettings<string>("ServerIp");
            ServerPort = configuration.GetAppSettings<int>("ServerPort");
            GateIp = configuration.GetAppSettings<string>("GateIp");
            GatePort = configuration.GetAppSettings<int>("GatePort");
            RedisHelper = new RedisCacheHelper(configuration);

            Loads = new Dictionary<string, Action>
            {
                {UsuallyDictionaryHelper.TableName, UsuallyDictionaryHelper.LoadConfig},
                {ScriptVersionHelper.TableName, ScriptVersionHelper.LoadConfig},
                {"ReadDB", LoadDateBase},
            };

            foreach (var action in Loads.Values)
            {
                action();
            }

            ClientManager.LoadConfig();
            
            Log.InfoFormat("ServerConfig Done, Count:{0}", ClientManager.GetDevices().Count());
        }

        public static void ReloadConfig(string tableName)
        {
            if (tableName != "all" && !Loads.ContainsKey(tableName))
            {
                return;
            }

            if (tableName == "all")
            {
                foreach (var action in Loads.Values)
                {
                    action();
                }
                ClientManager.UpdateConfig();
            }
            else
            {
                if (Loads.ContainsKey(tableName))
                {
                    Loads[tableName]();
                }
            }
        }

        private static void LoadDateBase()
        {
            var dbs = ApiDb.Query<ServerDataBase>("SELECT * FROM `management_database`;");
            var dataWrite = dbs.Where(x => x.Type == DataBaseType.Data && x.Write);
            if (dataWrite.Count() != 1)
            {
                throw new Exception($"LoadDateBase Write DataBase, {dataWrite.Count()}!!!");
            }

            DataWriteDb = new DataBase(dataWrite.First().DataBase);
        }
    }
}
