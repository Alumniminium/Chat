using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using GUI.Database;

namespace GUI.Networking.Entities
{
    public class VirtualServer
    {
        public int Id;
        public string Name { get; set; } = "";
        public string IconUrl { get; set; }
        public readonly Dictionary<int, User> Users;
        public readonly Dictionary<int, Channel> Channels;

        public Action<Channel> OnChannelAdded;
        public Action<Channel> OnChannelRemoved;
        public Action<User> OnUserAdded;
        public Action<User> OnUserRemoved;

        public VirtualServer()
        {
            Users = new Dictionary<int, User>();
            Channels = new Dictionary<int, Channel>();
        }

        public void AddUser(User user)
        {
            Users.Add(user.Id, user);
            OnUserAdded?.Invoke(user);
        }
        public void RemoveUser(User user)
        {
            Users.Remove(user.Id, out user);
            OnUserRemoved?.Invoke(user);
        }
        public void AddChannel(Channel channel)
        {
            Channels.Add(channel.Id, channel);
            OnChannelAdded?.Invoke(channel);
        }
        public void RemoveChannel(Channel channel)
        {
            Channels.Remove(channel.Id, out channel);
            OnChannelRemoved?.Invoke(channel);
        }


        public User GetUser(int id)
        {
            return Users[id];
        }
        public Channel GetChannel(int id)
        {
            return Channels[id];
        }

        public static VirtualServer CreateDMServer(User user)
        {
            var vs = new VirtualServer { Id = 0 };
            foreach (var (id, friend) in user.Friends)
            {
                vs.Channels.Add(id, new Channel(id, friend.Name));
                vs.AddUser(friend);
            }

            vs.IconUrl = user.AvatarUrl;
            vs.Name = "DM";

            return vs;
        }
    }
}
