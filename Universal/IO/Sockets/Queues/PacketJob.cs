using Universal.IO.Sockets.Client;

namespace Universal.IO.Sockets.Queues
{
    public class PacketJob
    {
        public readonly ClientSocket Client;
        public readonly byte[] Packet;

        public PacketJob(ClientSocket client, byte[] packet)
        {
            Client = client;
            Packet = packet;
        }
    }
}