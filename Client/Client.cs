using System;
using System.Collections.Generic;
using Client.Entities;
using Client.Networking;
using AlumniSocketCore.Client;
using AlumniSocketCore.Queues;
using Newtonsoft.Json;
using Client.IO.FastConsole;

namespace Client
{
    public class Client
    {
        [JsonIgnore]
        public ClientSocket Socket;
        public string Ip = "192.168.0.5";
        public ushort Port = 65534;
        public List<VirtualServer> Servers = new List<VirtualServer>();


        public void ConnectAsync(string ip, ushort port)
        {
            ReceiveQueue.Start(OnPacket);
            Socket = new ClientSocket(this);
            Socket.ShouldRetryConnecting=true;
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
            Socket.ConnectAsync(ip, port);
        }

        private void Connected() 
        {
            FastConsole.WriteLine("Connected!");
        }

        private void Disconnected() 
        {
            FastConsole.WriteLine("Disconnected!");
            ConnectAsync(Ip, Port);
        }

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketHandler.Handle((Client)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
