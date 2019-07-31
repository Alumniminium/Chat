using System;
using Client.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client.Networking.Handlers
{
    public static class MsgTextHandler
    {
        private static User User => Core.MyUser;
        public static void Process(byte[] buffer)
        {
            var msgTxt = (MsgText)buffer;

            if (msgTxt.ServerId == 0)
                HandleDirectMessage(msgTxt);
            else
                HandleServerMessage(msgTxt);

            FConsole.WriteLine($"MsgText: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }

        private static void HandleServerMessage(MsgText msgTxt)
        {
            if (!User.Servers.ContainsKey(msgTxt.ServerId))
                throw new ArgumentException("Username.Servers didn't contain " + msgTxt.ServerId);

            var server = User.Servers[msgTxt.ServerId];
            var channel = server.GetChannel(msgTxt.ChannelId);

            if (channel == null)
                throw new ArgumentException("server.Channels didn't contain " + msgTxt.ChannelId);

            foreach (var (key, value) in channel.Messages)
            {
                if (key != msgTxt.UniqueId)
                    continue;

                value.Text = msgTxt.GetText();
                return;
            }

            var message = Message.CreateFromMsg(msgTxt);
            channel.AddMessage(message);
        }

        private static void HandleDirectMessage(MsgText msgTxt)
        {
            if (!User.Friends.ContainsKey(msgTxt.AuthorId))
                throw new ArgumentException("Username.Friends didn't contain " + msgTxt.AuthorId);

            var friend = User.Friends[msgTxt.AuthorId];
            friend.LastActivity = DateTime.Now;

            var directMessageServer = User.Servers[0];
            var channel = directMessageServer.GetChannel(friend.Id);

            if (channel == null)
            {
                var friendChannel = new Channel(msgTxt.AuthorId, friend.Name);
                directMessageServer.AddChannel(friendChannel);
                channel = friendChannel;
            }

            foreach (var (key, value) in channel.Messages)
            {
                if (key != msgTxt.UniqueId)
                    continue;

                value.Text = msgTxt.GetText();
                return;
            }

            var message = Message.CreateFromMsg(msgTxt);
            channel.AddMessage(message);
        }
    }
}
