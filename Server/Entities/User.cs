using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using AlumniSocketCore.Client;

namespace Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Dictionary<int,VirtualServer> VirtualServers { get; set; }
        public Dictionary<int,User> Friends { get; set; }
        public ClientSocket Socket;

        public User()
        {
            VirtualServers = new Dictionary<int,VirtualServer>();
            Friends = new Dictionary<int,User>();
        }

        public void Send(byte[] packet) => Socket?.Send(packet);
        public string GetIp() => ((IPEndPoint) Socket.Socket.RemoteEndPoint).Address.ToString();
    }
}
