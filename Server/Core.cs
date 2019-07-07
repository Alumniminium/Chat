using Client.Database;
using Client.Entities;

namespace Client
{
    public static class Core
    {
        public static IDb Db = new JsonDb();
        public static ServerSettings Settings { get; set; }
    }
}