using System.Collections.Generic;

namespace Server.Entities
{
    public class Channel
    {
        public int Id{get;set;}
        public string Name {get;set;}
        public Dictionary<int,Message> Messages {get;set;}
    }
}
