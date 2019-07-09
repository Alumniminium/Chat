using Client.Networking;
using Universal.IO.Sockets.Client;
using Universal.IO.Sockets.Queues;
using Universal.IO.FastConsole;
using Universal.Packets;
using System.Threading;

namespace Client
{
    public class Client
    {
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
            FConsole.WriteLine("Connected!");
            Core.Client.Send(MsgLogin.Create("demo", "demo"));
        }

        private void Disconnected()
        {
            FConsole.WriteLine("Disconnected!");
            Socket.OnConnected -= Connected;
            Socket.OnDisconnect -= Disconnected;
            Thread.Sleep(5000);
            ConnectAsync();
        }

        private void OnPacket(ClientSocket client, byte[] buffer) => PacketHandler.Handle((Client)client.StateObject, buffer);

        public void Send(byte[] packet) => Socket.Send(packet);
    }
}
