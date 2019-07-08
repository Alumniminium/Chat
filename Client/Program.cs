using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client
{
    public static class Program
    {

        public static async Task Main()
        {
            FastConsole.Title = "CLIENT APP";

            AppDomain.CurrentDomain.UnhandledException += (sender, a) => Debugger.Break();

            LoadConfig();
            
            Core.Client.ConnectAsync(Core.SERVER_IP, Core.SERVER_PORT);

            while (!Core.Client.Socket.IsConnected)
                await Task.Delay(1);

            Core.Client.Send(MsgLogin.Create("demo", "demo"));

            while (true)
                FastConsole.ReadLine();
        }

        private static void LoadConfig()
        {
            if (File.Exists("config.json"))
                Core.Client = JsonConvert.DeserializeObject<Client>(File.ReadAllText("config.json"));
            else
                FastConsole.WriteLine("No config.json was found at " + Environment.CurrentDirectory + "/.config.json");
        }
    }
}