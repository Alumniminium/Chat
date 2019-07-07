using System.Collections.Generic;
using Server.Database.Entities;

namespace Server.Entities
{
    public class Channel
    {
        public int Id{get;set;}
        public string Name {get;set;}
        public List<Message> Messages {get;set;}
    }
}
