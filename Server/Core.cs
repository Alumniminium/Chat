using System;
using Server.Database;

namespace Server
{
    public static class Core
    {
        public static IDb Db = new JsonDb();
    }
}