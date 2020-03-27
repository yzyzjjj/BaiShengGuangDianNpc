using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NpcProxyLinkClient.Base.Logic;
using NpcProxyLinkClient.Base.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NpcProxyLinkClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            ServerConfig.Init(configuration);
            var cmds = new Dictionary<string, string>
            {
                { "0.exit","关闭服务器"},
                { "1.socket","查看socket"},
                { "2.client","查看Npc"},
                { "3.cmd","查看命令"},
                { "4.clear","清空"},
            };
            foreach (var cmd in cmds)
            {
                Console.WriteLine($"{cmd.Key}:{cmd.Value}");
            }
            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmds.All(x => x.Key.Split(".").All(y => y != cmd)))
                {
                    Console.WriteLine("what fuck");
                    continue;
                }

                var tCmd = cmds.First(x => x.Key.Split(".").Any(y => y == cmd)).Key.Split(".")[1];
                switch (tCmd)
                {
                    case "exit":
                        return;
                    case "clear":
                        Console.Clear();
                        foreach (var c in cmds)
                        {
                            Console.WriteLine($"{c.Key}:{c.Value}");
                        }
                        break;
                    case "cmd":
                        foreach (var c in cmds)
                        {
                            Console.WriteLine($"{c.Key}:{c.Value}");
                        }
                        break;
                    case "socket":
                        Console.WriteLine(ClientManager.SocketState);
                        break;
                    case "client":
                        foreach (var device in ClientManager.GetDevices())
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(device));
                            Console.WriteLine();
                        }
                        break;
                    default:
                        Console.WriteLine("what fuck");
                        break;
                }
            }
        }
    }
}
