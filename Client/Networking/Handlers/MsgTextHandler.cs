using System;
using Client.Entities;
using Universal.Packets;

namespace Client.Networking.Handlers
{
    public static class MsgTextHandler
    {
        // ! 
        private static User User => Core.MyUser;
        public static void Process(byte[] buffer)
        {
            var msgTxt = (MsgText)buffer;

            if (msgTxt.ServerId == 0)
                HandleDirectMessage(msgTxt);
            else
                HandleServerMessage(msgTxt);
        }

        private static void HandleServerMessage(MsgText msgTxt)
        {
            if (!User.Servers.ContainsKey(msgTxt.ServerId))
                throw new ArgumentException("user.Servers didn't contain " + msgTxt.ServerId);

            var server = User.Servers[msgTxt.ServerId];
            var channel = server.GetChannel(msgTxt.ChannelId);

            if (channel == null)
                throw new ArgumentException("server.Channels didn't contain " + msgTxt.ChannelId);

            var message = Message.CreateFromMsg(msgTxt);
            channel.AddMessage(message);
        }

        private static void HandleDirectMessage(MsgText msgTxt)
        {
            if (!User.Friends.ContainsKey(msgTxt.FriendId))
                throw new ArgumentException("user.Friends didn't contain " + msgTxt.FriendId);

            var friend = User.Friends[msgTxt.FriendId];
            friend.LastActivity = DateTime.Now;

            var directMessageServer = User.Servers[0];
            var channel = directMessageServer.GetChannel(friend.Id);

            if (channel == null)
            {
                var friendChannel = new Channel(msgTxt.FriendId, friend.Name);
                directMessageServer.AddChannel(friendChannel);
                channel = friendChannel;
            }

            var message = Message.CreateFromMsg(msgTxt);
            channel.AddMessage(message);
        }
    }
}
