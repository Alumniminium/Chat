
using Server.Entities;

namespace Server.Database
{
    public class JsonDb : IDb
    {
        public void EnsureDbReady()
        {

        }

        public bool UserExists(User user)
        {
            return false;
        }
        public bool AddUser(User user)
        {
            return true;
        }
        public bool Authenticate(ref User user)
        {
            return true;
        }

        public uint GetNextUniqueId()
        {
            return 0;
        }

        public uint GetNextServerUniqueId()
        {
            return 0;
        }
    }
}
