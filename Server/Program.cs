using System;
using Universal.IO.Sockets.Queues;
using Universal.IO.Sockets.Server;
using Server.Database;
using Server.Networking;
using Universal.Exceptions;
using Universal.IO.FastConsole;
using Universal.Performance;

namespace Server
{
    public static class Program
    {
        public static void Main()
        {
            FConsole.Title = "SERVER APP";
            GlobalExceptionHandler.Setup();
            Jit.PreJit();
            
            Core.Db = new JsonDb();
            Core.Db.EnsureDbReady();

            ReceiveQueue.Start(PacketRouter.Route);

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
    }
}
