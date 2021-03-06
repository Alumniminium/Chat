using System;
using System.Collections.Concurrent;
using Universal.Packets;

namespace Client.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Online { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public ConcurrentDictionary<int, User> Friends { get; set; }
        public ConcurrentDictionary<int, VirtualServer> Servers { get; set; }

        public DateTime LastActivity { get; set; }

        public User()
        {
            Friends = new ConcurrentDictionary<int, User>();
            Servers = new ConcurrentDictionary<int, VirtualServer>();
            Servers.TryAdd(0, new VirtualServer());
            Servers[0].Name = "Direct";
        }

        public VirtualServer GetServer(int serverId)
        {
            Servers.TryGetValue(serverId, out var server);
            return server;
        }
        public User GetFriend(int friendId)
        {
            if (friendId == Id)
                return this;
            Friends.TryGetValue(friendId, out var friend);
            return friend;
        }

        public void SendMessage(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                return;
            var channelId = 0;
            var serverId = 0;
            if(Core.SelectedChannel !=null)
                channelId = Core.SelectedChannel.Id;
            if(Core.SelectedServer != null)
                serverId = Core.SelectedServer.Id;
                
            Core.Client.Send(MsgText.Create(0, Id, input, serverId, channelId, DateTime.Now));
        }
    }
}