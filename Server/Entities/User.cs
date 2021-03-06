using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Universal.IO.Sockets.Client;
using Newtonsoft.Json;

namespace Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<int> VirtualServers { get; set; }
        public List<int> Friends { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ClientSocket Socket { get; set; }


        public User()
        {
            VirtualServers = new List<int>();
            Friends = new List<int>();
            VirtualServers.Add(0);
        }

        public void OnDisconnect()
        {
            Collections.OnlineUsers.TryRemove(Id, out var _);
        }

        public void Send(byte[] packet, bool dontCompress = false) => Socket?.Send(packet, dontCompress);
        public string GetIp() => ((IPEndPoint)Socket.Socket.RemoteEndPoint).Address.ToString();

        public override string ToString() => $"UserId: {Id} | Name: {Username} | Password: {Password} | Online: {Socket != null && Socket.IsConnected}";
    }
}
