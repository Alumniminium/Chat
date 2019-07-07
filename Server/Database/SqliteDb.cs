using System.Linq;
using Server.Entities;

namespace Server.Database
{
    public class SqliteDb : IDb
    {
        private readonly SquigglyContext db = new SquigglyContext();
        public void EnsureDbReady()
        {
            lock (db)
                db.Database.EnsureCreated();
        }

        public bool UserExists(User user)
        {
            lock (db)
            {
                var dbUser = db.Users.AsQueryable().FirstOrDefault(c => c.Username == user.Username);
                return dbUser != null;
            }
        }
        public bool AddUser(User user)
        {
            lock (db)
            {
                if (UserExists(user))
                    return false;
                user.Id = GetNextUniqueId();
                db.Users.Add(user);
                db.SaveChanges();
            }

            return true;
        }
        public bool Authenticate(ref User user)
        {
            lock (db)
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

        public uint GetNextUniqueId()
        {
            lock (db)
                return (uint)(db.Users.Count() + 1);
        }

        public uint GetNextServerUniqueId()
        {
            lock (db)
                return (uint)db.VirtualServers.Count() + 1;
        }
    }
}
