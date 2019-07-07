using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using AlumniSocketCore.Client;

namespace Server.Database.Models
{
    public class User
    {
        [Key]
        public ulong UniqueId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<ulong> Servers { get; set; }

        [NotMapped]
        public ClientSocket Socket;

        public User()
        {
            Servers = new List<ulong>();
        }

        public void Send(byte[] packet)
        {
            Socket?.Send(packet);
        }

        public string GetIp() => ((IPEndPoint) Socket.Socket.RemoteEndPoint).Address.ToString();
    }
}
