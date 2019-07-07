using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Client.IO.FastConsole
{
    public static class FastConsoleThread
    {
        internal static Thread WorkerThread;
        internal static ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();
        internal static AutoResetEvent Block = new AutoResetEvent(false);

        static FastConsoleThread()
        {
            WorkerThread =new Thread(WorkLoop);
            WorkerThread.IsBackground=true;
            WorkerThread.Start();
        }
        
        public static void Add(string msg)
        {
            Queue.Enqueue(msg);
            Block.Set();
        }

        private static void WorkLoop()
        {
            while(true)
            {
                Block.WaitOne();
                while(Queue.TryDequeue(out var msg))
                {
                    Console.WriteLine(msg);
                }
            }
        }
    }
}