using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Universal.IO.Sockets.Queues;
using Universal.IO.Sockets.Server;
using Server.Database;
using Server.Networking;
using Universal.IO.FastConsole;

namespace Server
{
    public static class Program
    {
        public static void Main()
        {
            FastConsole.Title = "SERVER APP";

            SetupCountermeasuresForShitCode();

            Core.Db = new JsonDb();
            Core.Db.EnsureDbReady();

            ReceiveQueue.Start(PacketHandler.Handle);

            ServerSocket.Start(Core.Settings.Port);

            FastConsole.WriteLine("Online");

            while (true)
            {
                var cmd = FastConsole.ReadLine();
                switch (cmd)
                {
                    case "exit":
                        Core.Db.Save();
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void SetupCountermeasuresForShitCode()
        {
            TaskScheduler.UnobservedTaskException += (_, exception) =>
            {
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.Message}");
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.StackTrace}");
                exception.SetObserved();
                Debugger.Break();
            };
            AppDomain.CurrentDomain.UnhandledException += (_, exception) =>
            {
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.Message}");
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.StackTrace}");
                Debugger.Break();
            };
        }
    }
}
