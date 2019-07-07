using System.Collections.Generic;
using Client.Database.Entities;

namespace Client.Entities
{
    public class Channel
    {
        public int Id{get;set;}
        public string Name {get;set;}
        public List<Message> Messages {get;set;}
    }
}
