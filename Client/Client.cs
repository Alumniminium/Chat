using System.Collections.Generic;
using Client.Entities;
using Client.Networking;
using AlumniSocketCore.Client;
using AlumniSocketCore.Queues;
using Newtonsoft.Json;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client
{
    public class Client
    {
        public string Ip = "192.168.0.5";
        public ushort Port = 65534;
        public readonly Dictionary<int,VirtualServer> Servers = new Dictionary<int, VirtualServer>();

        [JsonIgnore]
        public ClientSocket Socket;

        public User Me;

        public void ConnectAsync(string ip, ushort port)
        {
            ReceiveQueue.Start(OnPacket);

            Socket = new ClientSocket(this) { ShouldRetryConnecting = true };

            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;

            Socket.ConnectAsync(ip, port);
        }

        private void Connected()
        {
            FastConsole.WriteLine("Connected!");
            Core.Client.Send(MsgLogin.Create("demo", "demo"));
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
