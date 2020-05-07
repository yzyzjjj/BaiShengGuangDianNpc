using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaiShengGuangDianWeb.Base.Helper;
using Microsoft.Extensions.Configuration;
using ModelBase.Base.Dapper;
using ModelBase.Base.Logger;
using ModelBase.Base.Utils;
using NpcProxyLink.Base.Helper;
using NpcProxyLink.Base.Logic;

namespace NpcProxyLink.Base.Server
{
    public class ServerConfig
    {
        public static DataBase ApiDb;
        public static ClientManager ClientManager;
        public static int ServerId;
        public static RedisCacheHelper RedisHelper;
        public static Dictionary<string, Action> Loads;

        public static void Init(IConfiguration configuration)
        {
            ApiDb = new DataBase(configuration.GetConnectionString("ApiDb"));
            ServerId = configuration.GetAppSettings<int>("ServerId");

            RedisHelper = new RedisCacheHelper(configuration);
            Loads = new Dictionary<string, Action>
            {
                {UsuallyDictionaryHelper.TableName, UsuallyDictionaryHelper.LoadConfig},
                {ScriptVersionHelper.TableName, ScriptVersionHelper.LoadConfig},
            };

            foreach (var action in Loads.Values)
            {
                action();
            }
            ClientManager = new ClientManager();
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
    }
}
