using Client.Database;

namespace Client
{
    public static class Core
    {
        public static IDb Db = new JsonDb();
    }
}