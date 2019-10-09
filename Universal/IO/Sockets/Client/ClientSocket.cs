﻿using Universal.IO.Sockets.Queues;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Universal.IO.FastConsole;
using System.Runtime.InteropServices;
using Universal.Extensions;

namespace Universal.IO.Sockets.Client
{
    public class ClientSocket
    {
        public Action OnConnected, OnDisconnect;

        public Socket Socket;
        public object StateObject;
        public bool IsConnected;
        public bool UseCompression { get; set; }

        internal readonly NeutralBuffer Buffer;
        internal readonly AutoResetEvent SendSync = new AutoResetEvent(true);
        internal readonly AutoResetEvent ReceiveSync = new AutoResetEvent(false);
        internal readonly SocketAsyncEventArgs ReceiveArgs;
        internal readonly SocketAsyncEventArgs SendArgs;

        public ClientSocket(object stateObject, bool useCompression = false)
        {
            Buffer = new NeutralBuffer();
            StateObject = stateObject;
            UseCompression = useCompression;

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
        public void Send(byte[] packet, bool dontCompress = false)
        {
            SendSync.WaitOne();

            var size = packet.Length;

            if (UseCompression && !dontCompress)
                size = Compress(packet);
            else
            {
                //ref var packetRef = ref MemoryMarshal.GetReference(packet.AsSpan());
                //ref var sendBufferRef = ref MemoryMarshal.GetReference(Buffer.SendBuffer.AsSpan());
                //System.Runtime.CompilerServices.Unsafe.CopyBlock(ref sendBufferRef, ref packetRef, (uint)packet.Length);
                packet.VectorizedCopy(0, Buffer.SendBuffer, 0, packet.Length);
            }

            SendArgs.SetBuffer(Buffer.SendBuffer, 0, size);

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

        private int Compress(byte[] packet)
        {
            using (var ms = new MemoryStream())
            using (var cp = new DeflateStream(ms, CompressionMode.Compress))
            {
                ms.Seek(2, SeekOrigin.Begin);
                cp.Write(packet);
                cp.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                ms.Write(BitConverter.GetBytes((short)ms.Length));

                var size = (int)ms.Length;
                var cpacket = ms.ToArray();//.VectorizedCopy(0, Buffer.SendBuffer, 0, size);

                ref var cpacketRef = ref MemoryMarshal.GetReference(cpacket.AsSpan());
                ref var sendBufferRef = ref MemoryMarshal.GetReference(Buffer.SendBuffer.AsSpan());
                System.Runtime.CompilerServices.Unsafe.CopyBlock(ref sendBufferRef, ref cpacketRef, (uint)size);
                return size;
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
                FConsole.WriteLine("Disconnecting: " + reason);
                IsConnected = false;
            }
            finally
            {
                OnDisconnect?.Invoke();
            }
        }
    }
}
