using System;
using System.Diagnostics;
using Server.Database;
using Server.Networking;
using AlumniSocketCore.Queues;
using AlumniSocketCore.Server;
using System.Threading.Tasks;

namespace Server
{
    public static class Program
    {
        public static void Main()
        {
            Console.Title = "SERVER APP";
            SetupCountermeasuresForShitCode();

            Core.Db.EnsureDbReady();

            ReceiveQueue.Start(PacketHandler.Handle);
            ServerSocket.Start(65534);
            Console.WriteLine("Online");
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void SetupCountermeasuresForShitCode()
        {
            TaskScheduler.UnobservedTaskException += (_, exception) =>
            {
                Console.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.Message}");
                Console.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.StackTrace}");
                exception.SetObserved();
                Debugger.Break();
            };
            AppDomain.CurrentDomain.UnhandledException += (_, exception) =>
            {
                Console.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception).Message}");
                Console.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception).StackTrace}");
                Debugger.Break();
            };
        }
    }
}
