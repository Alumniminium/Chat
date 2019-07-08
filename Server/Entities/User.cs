using System;
using System.Collections.Generic;
using System.Net;
using AlumniSocketCore.Client;
using Newtonsoft.Json;

namespace Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<int> VirtualServers { get; set; }
        public List<int> Friends { get; set; }
        [JsonIgnore]
        public ClientSocket Socket { get; set; }

        public User()
        {
            VirtualServers = new List<int>();
            Friends = new List<int>();
        }

        public void Send(byte[] packet) => Socket?.Send(packet);
        public string GetIp() => ((IPEndPoint) Socket.Socket.RemoteEndPoint).Address.ToString();

        public override string ToString() => $"UserId: {Id} | Username: {Username} | Password: {Password} | Online: {Socket.IsConnected}";
    }
}
