using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Client.IO.FastConsole;
using Newtonsoft.Json;
using Packets;

namespace Client
{
    public static class Program
    {
        public static User Client = new User();
        public const string SERVER_IP = "192.168.0.3";
        public const ushort SERVER_PORT = 65534;

        public static async Task Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, a) =>
            {
                Debugger.Break();
            };
            FastConsole.Title = "CLIENT APP";
            LoadConfig();
            
            Client.ConnectAsync(SERVER_IP, SERVER_PORT);

            while (!Client.Socket.IsConnected)
                await Task.Delay(1);

            Client.Send(MsgLogin.Create("user", "pass"));

            while (true)
                FastConsole.ReadLine();
        }

        private static void LoadConfig()
        {
            if (File.Exists("config.json"))
                Client = JsonConvert.DeserializeObject<User>(File.ReadAllText("config.json"));
            else
                FastConsole.WriteLine("No config.json was found at " + Environment.CurrentDirectory + "/.config.json");
        }
    }
}