using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Universal.IO.FastConsole
{
    public static class FastConsoleThread
    {
        internal static readonly Thread WorkerThread;
        internal static readonly ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();
        internal static readonly AutoResetEvent Block = new AutoResetEvent(false);

        static FastConsoleThread()
        {
            WorkerThread = new Thread(WorkLoop) {IsBackground = true};
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