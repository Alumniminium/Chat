﻿using Client.Networking;
using Universal.IO.Sockets.Client;
using Universal.IO.Sockets.Queues;
using Universal.IO.FastConsole;
using Universal.Packets;
using System.Threading;
using Universal.Packets.Enums;

namespace Client
{
    public class Client
    {
        public ClientSocket Socket;
        public string User;
        public string Pass;
        public void ConnectAsync(string user, string pass)
        {
            User = user;
            Pass = pass;
            ReceiveQueue.OnPacket += OnPacket;

            Socket = new ClientSocket(this);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
            Socket.ConnectAsync(Core.SERVER_IP, Core.SERVER_PORT);
        }

        private void Connected()
        {
            FConsole.WriteLine("Connected!");
            Core.Client.Send(MsgLogin.Create(User, Pass, "", false, MsgLoginType.Login));
        }

        private void Disconnected()
        {
            FConsole.WriteLine("Disconnected!");
            Socket.OnConnected -= Connected;
            Socket.OnDisconnect -= Disconnected;
            Socket?.Socket?.Dispose();
            Thread.Sleep(5000);
            ConnectAsync(User, Pass);
        }

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketRouter.Route((Client)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
