using System.Linq;
using Server.Entities;

namespace Server.Database
{
    public class SqliteDb : IDb
    {
        private readonly SquigglyContext _db = new SquigglyContext();
        public void EnsureDbReady()
        {
            lock (_db)
                _db.Database.EnsureCreated();
        }

        public bool UserExists(User user)
        {
            lock (_db)
            {
                var dbUser = _db.Users.AsQueryable().FirstOrDefault(c => c.Username == user.Username);
                return dbUser != null;
            }
        }
        public bool AddUser(User user)
        {
            lock (_db)
            {
                if (UserExists(user))
                    return false;
                user.Id = GetNextUserId();
                _db.Users.Add(user);
                _db.SaveChanges();
            }

            return true;
        }
        public bool Authenticate(ref User user)
        {
            lock (_db)
            {
                var username = user.Username;
                var dbUser = _db.Users.AsQueryable().FirstOrDefault(c => c.Username == username);

                if (dbUser == null || dbUser.Password != user.Password)
                    return false;

                dbUser.Socket = user.Socket;
                user = dbUser;
                user.Socket.StateObject = dbUser;
                return true;
            }
        }

        public int GetNextUserId()
        {
            lock (_db)
                return (_db.Users.Count() + 1);
        }

        public int GetNextServerId()
        {
            lock (_db)
                return _db.VirtualServers.Count() + 1;
        }

        public void Save()
        {
            
        }
    }
}
