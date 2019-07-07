using System.Linq;
using Server.Database.Models;

namespace Server.Database
{
    public static class Db
    {
        private static readonly SquigglyContext db = new SquigglyContext();
        internal static void EnsureDb()
        {
            lock(db)
                db.Database.EnsureCreated();
        }

        public static bool UserExists(User user)
        {
            lock(db)
            {
                var dbUser = db.Users.AsQueryable().FirstOrDefault(c => c.Username == user.Username);
                return dbUser != null;
            }
        }
        public static bool AddUser(User user)
        {
            lock(db)
            {
                if (UserExists(user))
                    return false;
                user.UniqueId = GetNextUniqueId();
                db.Users.Add(user);
                db.SaveChanges();
            }

            return true;
        }
        public static bool Authenticate(ref User user)
        {
            lock(db)
            {
                var username = user.Username;
                var dbUser = db.Users.AsQueryable().FirstOrDefault(c => c.Username == username);
                if (dbUser != null && dbUser.Password == user.Password)
                {
                    dbUser.Socket = user.Socket;
                    user = dbUser;
                    user.Socket.StateObject = dbUser;
                    return true;
                }
            }
            return false;
        }

        public static ulong GetNextUniqueId()
        {
            lock(db)
                return (ulong)(db.Users.Count() + 1);
        }

        public static int GetNextSubdomainUniqueId()
        {
            lock(db)
                return db.VirtualServers.Count() + 1;
        }

    }
}
