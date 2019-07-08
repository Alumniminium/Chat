using Server.Database;
using Server.Entities;

namespace Server
{
    public static class Core
    {
        public static IDb Db = new JsonDb();
        public static ServerSettings Settings { get; set; }
    }
}