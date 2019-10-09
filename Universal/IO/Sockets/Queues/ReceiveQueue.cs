using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Universal.Extensions;
using Universal.IO.Sockets.Client;

namespace Universal.IO.Sockets.Queues
{
    public static class ReceiveQueue
    {
        private static readonly BlockingCollection<SocketAsyncEventArgs>[] Queue = new BlockingCollection<SocketAsyncEventArgs>[Environment.ProcessorCount];
        private static Thread[] _workerThread = new Thread[Environment.ProcessorCount];
        private const int MIN_HEADER_SIZE = 2;

        static ReceiveQueue()
        {
            for (int i = 0; i < 1; i++)
            {
                Queue[i] = new BlockingCollection<SocketAsyncEventArgs>();
                _workerThread[i] = new Thread(WorkLoop);
                _workerThread[i].Priority = ThreadPriority.AboveNormal;
                _workerThread[i].IsBackground = true;
                _workerThread[i].Start(i);
            }
        }

        public static void Add(SocketAsyncEventArgs e)
        {
            var queueId = 0;//Queue.Min(q => q.Count);
            Queue[queueId].Add(e);
        }

        private static void WorkLoop(object queueId)
        {
            int id = (int)queueId;
            foreach (var e in Queue[id].GetConsumingEnumerable())
            {
                var cli = (ClientSocket)e.UserToken;
                if (cli.Buffer.Ready)
                    AssemblePacket(e);
                else
                    Add(e);
            }
        }
        private static void AssemblePacket(SocketAsyncEventArgs e)
        {
            while (true)
            {
                var connection = (ClientSocket)e.UserToken;

                if (connection.Buffer.BytesInBuffer == 0)
                    StartNewPacket(e, connection);
                if (connection.Buffer.BytesInBuffer > 0)
                    ReadHeader(e, connection);

                MergeUnsafe(e);

                if (connection.Buffer.BytesInBuffer == connection.Buffer.BytesRequired && connection.Buffer.BytesRequired > 4)
                {
                    FinishPacket(connection);
                    Add(e);
                    break;
                }
                if (connection.Buffer.BytesProcessed != e.BytesTransferred)
                    continue;

                connection.Buffer.BytesProcessed = 0;
                break;
            }
        }

        private static void StartNewPacket(SocketAsyncEventArgs e, ClientSocket connection)
        {
            var receivedBytes = e.BytesTransferred - connection.Buffer.BytesProcessed;
            if (receivedBytes >= MIN_HEADER_SIZE)
                connection.Buffer.BytesRequired = BitConverter.ToInt16(e.Buffer, connection.Buffer.BytesProcessed);
        }

        private static void ReadHeader(SocketAsyncEventArgs e, ClientSocket connection)
        {
            if (connection.Buffer.BytesInBuffer < MIN_HEADER_SIZE)
                MergeUnsafe(e, true);
            else
                connection.Buffer.BytesRequired = BitConverter.ToInt16(connection.Buffer.MergeBuffer, 0);
        }

        private static void FinishPacket(ClientSocket connection)
        {
            if (connection.UseCompression)
                Decompress(connection);

            ProcessingQueue.Enqueue(connection);
            connection.Buffer.BytesInBuffer = 0;
        }

        private static void Decompress(ClientSocket connection)
        {
            var compressedArray = new byte[connection.Buffer.BytesRequired];
            connection.Buffer.MergeBuffer.VectorizedCopy(MIN_HEADER_SIZE, compressedArray, 0, compressedArray.Length - MIN_HEADER_SIZE);

            using (var compressedStream = new MemoryStream(compressedArray))
            using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                deflateStream.Read(connection.Buffer.MergeBuffer, 0, connection.Buffer.MergeBuffer.Length);
            }
        }


        private static unsafe void MergeUnsafe(SocketAsyncEventArgs e, bool header = false)
        {
            var connection = (ClientSocket)e.UserToken;
            var _count = e.BytesTransferred - connection.Buffer.BytesProcessed;
            var _destOffset = connection.Buffer.BytesInBuffer;
            var _recOffset = connection.Buffer.BytesProcessed;

            fixed (byte* destination = connection.Buffer.MergeBuffer)
            fixed (byte* source = e.Buffer)
            {
                for (var i = 0; i < _count; i++)
                {
                    destination[i + _destOffset] = source[i + _recOffset];
                    connection.Buffer.BytesInBuffer++;
                    connection.Buffer.BytesProcessed++;

                    if (header && connection.Buffer.BytesInBuffer == MIN_HEADER_SIZE)
                        break;

                    if (!header && connection.Buffer.BytesInBuffer == connection.Buffer.BytesRequired)
                        break;
                }
            }
        }
    }
}