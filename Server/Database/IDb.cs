using Server.Entities;

namespace Server.Database
{
    public interface IDb
    {
        void EnsureDbReady();
        bool UserExists(string user);
        bool AddUser(User user);
        bool Authenticate(string user, string pass);
        User GetDbUser(string username);
        int GetNextUserId();
        int GetNextServerId();
        void Save();
        void LoadUser(ref User user);
    }
}
