using Server.Database;
using Server.Entities;

namespace Server
{
    public static class Core
    {
        public static JsonDb Db = new JsonDb();
        public static ServerSettings Settings { get; set; }
    }
}