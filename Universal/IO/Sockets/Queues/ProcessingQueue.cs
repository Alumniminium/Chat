using System;
using System.Threading;
using System.Collections.Concurrent;
using Universal.IO.Sockets.Client;

namespace Universal.IO.Sockets.Queues
{
    public static class ProcessingQueue
    {
        private static BlockingCollection<ClientSocket> pendingJobs = new BlockingCollection<ClientSocket>();
        private static Thread worker = new Thread(WorkLoop);
        public static Action<ClientSocket, byte[]> onPacket;

        static ProcessingQueue()
        {
            worker.IsBackground = true;
            worker.Priority = ThreadPriority.Highest;
            worker.Start();
        }

        public static void Enqueue(ClientSocket item) => pendingJobs.Add(item);
        private static void WorkLoop()
        {
            foreach (var job in pendingJobs.GetConsumingEnumerable())
            {
                onPacket?.Invoke(job, job.Buffer.MergeBuffer);
                job.Buffer.Ready = true;
                job.ReceiveSync.Set();
            }
        }
    }
}