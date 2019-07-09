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
            FConsole.Title = "SERVER APP";

            SetupCountermeasuresForShitCode();

            Core.Db = new JsonDb();
            Core.Db.EnsureDbReady();

            ReceiveQueue.Start(PacketHandler.Handle);

            ServerSocket.Start(Core.Settings.Port);

            FConsole.WriteLine("Online");

            while (true)
            {
                var cmd = FConsole.ReadLine();
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
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.Message}");
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.StackTrace}");
                exception.SetObserved();
                Debugger.Break();
            };
            AppDomain.CurrentDomain.UnhandledException += (_, exception) =>
            {
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.Message}");
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.StackTrace}");
                Debugger.Break();
            };
        }
    }
}
