using System.Threading;
using AvaloniaMVVMClient.Networking.Networking;
using Universal.IO.FastConsole;
using Universal.IO.Sockets.Client;
using Universal.IO.Sockets.Queues;
using Universal.Packets;
using Universal.Packets.Enums;

namespace AvaloniaMVVMClient.Networking
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
            ReceiveQueue.Start(OnPacket);

            Socket = new ClientSocket(this);
            Socket.OnDisconnect += Disconnected;
            Socket.OnConnected += Connected;
            Socket.ConnectAsync(Core.SERVER_IP, Core.SERVER_PORT);
        }

        private void Connected()
        {
            FConsole.WriteLine("Connected!");
            Core.Client.Send(MsgLogin.Create(User, Pass, "", true, MsgLoginType.Login));
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
