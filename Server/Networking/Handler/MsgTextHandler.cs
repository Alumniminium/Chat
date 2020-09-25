using System.Diagnostics;
using Server.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgTextHandler
    {
        public static void Process(User user, byte[] buffer)
        {
            var msgTxt = (MsgText)buffer;

            if (msgTxt.ServerId == 0)
                HandleDirectMessage(msgTxt);
            else
                HandleServerMessage(msgTxt);

            FConsole.WriteLine($"MsgLogin Deserializing & Processing took {((((float)PacketRouter.Stopwatch.ElapsedTicks) / Stopwatch.Frequency) * 1000000):0}microsec");
        }

        private static void HandleServerMessage(MsgText msgTxt)
        {
            var server = Oracle.GetServerFromId(msgTxt.ServerId);
            var channel = Oracle.GetServerChannelFromId(server.Id, msgTxt.ChannelId);

            var message = Message.FromMsg(msgTxt);
            channel.Messages.Add(message.Id, message);

            foreach (var (_, user) in Collections.OnlineUsers)
            {
                user.Socket?.Send(MsgText.Create(message.Id, message.AuthorId, message.Text, server.Id, channel.Id, message.Timestamp));
            }
        }

        private static void HandleDirectMessage(MsgText msgTxt)
        {
            var friend = Oracle.GetUserFromId(msgTxt.FriendId);
            friend?.Send(msgTxt);
        }
    }
}
