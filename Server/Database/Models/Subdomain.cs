using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.Models
{
    public class VirtualServer
    {
        [Key]
        public int UniqueId { get; set; }
        public ulong OwnerId { get; set; }
        public string Name { get; set; }
        public List<ulong> Users{get;set;}
        public List<Channel> Channels{get;set;}
        public DateTime LastUpdate { get; set; }

        public VirtualServer()
        {
            LastUpdate = DateTime.Now;
        }
    }
}
