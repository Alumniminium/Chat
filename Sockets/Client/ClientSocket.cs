using Sockets.Queues;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sockets.Client
{
    public class ClientSocket
    {
        public Action OnConnected, OnDisconnect;

        public Socket Socket;
        public object StateObject;
        public bool IsConnected;

        internal readonly NeutralBuffer Buffer;
        internal readonly AutoResetEvent SendSync = new AutoResetEvent(true);
        internal readonly AutoResetEvent ReceiveSync = new AutoResetEvent(false);
        internal readonly SocketAsyncEventArgs ReceiveArgs;
        internal readonly SocketAsyncEventArgs SendArgs;

        public ClientSocket(object stateObject)
        {
            Buffer = new NeutralBuffer();
            StateObject = stateObject;

            SendArgs = new SocketAsyncEventArgs();
            SendArgs.Completed += Sent;

            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.SetBuffer(Buffer.ReceiveBuffer, 0, Buffer.ReceiveBuffer.Length);
            ReceiveArgs.Completed += Received;
            ReceiveArgs.UserToken = this;
        }


        public void ConnectAsync(string host, ushort port)
        {
            try
            {
                if (IsConnected)
                    Disconnect("ConnectAsync() IsConnected == true");

                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var connectArgs = new SocketAsyncEventArgs
                {
                    RemoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port)
                };
                connectArgs.Completed += Connected;
                if (!Socket.ConnectAsync(connectArgs))
                    Connected(null, connectArgs);
            }
            catch (Exception e)
            {
                Disconnect("ConnectAsync() Catch: " + e.Message + " #### " + e.StackTrace);
            }
        }

        private void Connected(object o, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                IsConnected = true;
                try
                {
                    OnConnected?.Invoke();
                    if (!Socket.ReceiveAsync(ReceiveArgs))
                        Received(null, ReceiveArgs);
                }
                catch (Exception ex)
                {
                    Disconnect("ClientSocket.Connected() Catch: " + ex.Message + " #### " + ex.StackTrace);
                }
            }
            else
                Disconnect("ClientSocket.Connected() e.SocketError != SocketError.Success");
        }

        private void Received(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)
            {
                Disconnect("ClientSocket.Received() if (e.SocketError != SocketError.Success || e.BytesTransferred == 0)");
                return;
            }

            try
            {
                ReceiveQueue.Add(e);
                ReceiveSync.WaitOne();
                if (!Socket.ReceiveAsync(e))
                    Received(null, e);
            }
            catch (Exception ex)
            {
                Disconnect("ClientSocket.Received() Catch: " + ex.Message + " #### " + ex.StackTrace);
            }
        }
        public void Send(byte[] packet)
        {
            SendSync.WaitOne();

            using (var stream = new MemoryStream())
            using (var compression = new DeflateStream(stream, CompressionLevel.Optimal))
            {
                compression.Write(packet);
                compression.Flush();

                var compressedPacket = new byte[stream.Length + 4];
                var compressedData = stream.ToArray();
                var compressedLengthBytes = BitConverter.GetBytes((short)(compressedPacket.Length));

                System.Buffer.BlockCopy(compressedLengthBytes, 0, compressedPacket, 0, compressedLengthBytes.Length);
                System.Buffer.BlockCopy(compressedData, 0, compressedPacket, 4, compressedData.Length);

                Console.WriteLine($"Compression Efficiency for {BitConverter.ToUInt16(packet, 4)} ({packet.Length} vs {compressedPacket.Length} bytes) Ratio: " + compressedPacket.Length / (float)packet.Length * 100);
            }

            SendArgs.SetBuffer(packet, 0, packet.Length);

            try
            {
                if (!Socket.SendAsync(SendArgs))
                    Sent(null, SendArgs);
            }
            catch (Exception e)
            {
                Disconnect("ClientSocket.Send() Catch: " + e.Message + " #### " + e.StackTrace);
            }
        }

        private void Sent(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
                SendSync.Set();
            else
                Disconnect("ClientSocket.Sent() e.SocketError != SocketError.Success");
        }
        public void Disconnect(string reason)
        {
            try
            {
                Console.WriteLine("Disconnecting: " + reason);
                IsConnected = false;
            }
            finally
            {
                OnDisconnect?.Invoke();
            }
        }
    }
}
