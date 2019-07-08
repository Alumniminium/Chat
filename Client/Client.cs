using System.Collections.Generic;
using Client.Entities;
using Client.Networking;
using Sockets.Client;
using Sockets.Queues;
using Newtonsoft.Json;
using Universal.IO.FastConsole;
using Universal.Packets;
using System.Threading;

namespace Client
{
    public class Client
    {
        [JsonIgnore]
        public ClientSocket Socket;

        public void ConnectAsync()
        {
            ReceiveQueue.Start(OnPacket);

            Socket = new ClientSocket(this);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;

            Socket.ConnectAsync(Core.SERVER_IP, Core.SERVER_PORT);
        }

        private void Connected()
        {
            FastConsole.WriteLine("Connected!");
            Core.Client.Send(MsgLogin.Create("demo", "demo"));
        }

        private void Disconnected()
        {
            FastConsole.WriteLine("Disconnected!");
            Socket.OnConnected-=Connected;
            Socket.OnDisconnect-=Disconnected;
            Thread.Sleep(5000);
            ConnectAsync();
        }

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketHandler.Handle((Client)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
