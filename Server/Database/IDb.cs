using Server.Entities;

namespace Server.Database
{
    public interface IDb
    {
        void EnsureDbReady();
        bool UserExists(User user);
        bool AddUser(User user);
        bool Authenticate(ref User user);
        int GetNextUserId();
        int GetNextServerId();
        void Save();
    }
}
