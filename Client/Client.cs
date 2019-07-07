using System.Collections.Generic;
using AlumniClient.Models;
using AlumniClient.Networking;
using AlumniSocketCore.Client;
using AlumniSocketCore.Queues;
using Newtonsoft.Json;

namespace AlumniClient
{
    public class Client
    {
        [JsonIgnore]
        public ClientSocket Socket;
        public bool IsConnected;
        public string Ip = "192.168.0.3";
        public ushort Port = 65534;
        public List<Server> Servers = new List<Server>();


        public void ConnectAsync(string ip, ushort port)
        {
            ReceiveQueue.Start(OnPacket);
            Socket = new ClientSocket(this);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
            Socket.ConnectAsync(ip, port);
        }

        private void Connected() => IsConnected=true;

        private void Disconnected() => ConnectAsync(Ip, Port);

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketHandler.Handle((Client)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
