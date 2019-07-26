using Client.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client.Networking.Handlers
{
    public static class MsgChannelHandler
    {
        public static User User => Core.MyUser;
        public static void Process(byte[] buffer)
        {
            var msgChannel = (MsgChannel)buffer;
            var channel = Channel.FromMsg(msgChannel);

            var server = User.GetServer(msgChannel.ServerId);

            if (server == null)
                throw new System.ArgumentException("server is null");

            server.AddChannel(channel);

            if (Core.SelectedServer.Id == server.Id)
                Core.SelectedChannel = channel;

            FConsole.WriteLine($"Received Server info for {channel.Name}!");
            FConsole.WriteLine($"MsgChannel: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}
