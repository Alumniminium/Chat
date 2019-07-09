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

        public bool UserExists(string user)
        {
            lock (_db)
            {
                return _db.Users.AsQueryable().Any(c => c.Username == user);
            }
        }
        public bool AddUser(User user)
        {
            lock (_db)
            {
                if (UserExists(user.Username))
                    return false;

                _db.Users.Add(user);

                _db.SaveChanges();
            }

            return true;
        }
        public bool Authenticate(string username, string password)
        {
            lock (_db)
            {
                return _db.Users.AsQueryable().Any(c => c.Username == username && c.Password == password);
            }
        }
        public User GetDbUser(string username)
        {
            if (!UserExists(username))
                return null;
            lock (_db)
                return _db.Users.First(u => u.Username == username);
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

        public void LoadUser(ref User user)
        {
            
        }
    }
}
