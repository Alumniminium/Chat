using System.Collections.Generic;

namespace AlumniClient.Models
{
    public class User
    {
        public ulong Id;
        public string Name;
        public bool Online;
        public List<Message> Messages;
    }
}