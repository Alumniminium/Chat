using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading;
using Universal.Extensions;
using Universal.IO.Sockets.Client;

namespace Universal.IO.Sockets.Queues
{
    public static class ReceiveQueue
    {
        private static readonly ConcurrentQueue<SocketAsyncEventArgs> Queue = new ConcurrentQueue<SocketAsyncEventArgs>();
        private static readonly AutoResetEvent Sync = new AutoResetEvent(false);
        private static Thread _workerThread;
        private static Action<ClientSocket, byte[]> _onPacket;
        private const int MIN_HEADER_SIZE = 2;
        private static int _count;
        private static int _destOffset;
        private static int _recOffset;

        public static void Start(Action<ClientSocket, byte[]> onPacket)
        {
            if (_onPacket == null)
                _onPacket = onPacket;
            if (_workerThread == null)
                _workerThread = new Thread(WorkLoop) { IsBackground = true, Priority = ThreadPriority.Highest };
            if (!_workerThread.IsAlive)
                _workerThread.Start();
        }

        public static void Add(SocketAsyncEventArgs e)
        {
            Queue.Enqueue(e);
            Sync.Set();
        }

        private static void WorkLoop()
        {
            while (true)
            {
                Sync.WaitOne();
                while (Queue.TryDequeue(out var e))
                {
                    var connection = (ClientSocket)e.UserToken;

                    if (connection == null)
                        continue;

                    AssemblePacket(e);

                    connection.ReceiveSync.Set();
                }
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
                    FinishPacket(connection);

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
            {
                _count = MIN_HEADER_SIZE - connection.Buffer.BytesInBuffer;
                MergeUnsafe(e, true);
            }
            else
                connection.Buffer.BytesRequired = BitConverter.ToInt16(connection.Buffer.MergeBuffer, 0);
        }

        private static void FinishPacket(ClientSocket connection)
        {
            if (connection.UseCompression)
                Decompress(connection);

            _onPacket(connection, connection.Buffer.MergeBuffer);
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
            _count = e.BytesTransferred - connection.Buffer.BytesProcessed;
            _destOffset = connection.Buffer.BytesInBuffer;
            _recOffset = connection.Buffer.BytesProcessed;

            fixed (byte* destination = connection.Buffer.MergeBuffer)
            fixed (byte* source = e.Buffer)
            {
                for (var i = 0; i < _count; i++)
                {
                    destination[i + _destOffset] = source[i + _recOffset];
                    connection.Buffer.BytesInBuffer++;
                    connection.Buffer.BytesProcessed++;

                    if(header && connection.Buffer.BytesInBuffer == MIN_HEADER_SIZE)
                        break;

                    if (!header && connection.Buffer.BytesInBuffer == connection.Buffer.BytesRequired)
                        break;
                }
            }
        }
    }
}