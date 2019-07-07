using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using Client.Entities;

namespace Client.Database
{
    public class JsonDb : IDb
    {
        const string VIRTUAL_SERVERS_FILE = "../../../../VirtualServers.json";
        const string USERS_FILE = "../../../../Users.json";
        
        public void EnsureDbReady()
        {
            if (File.Exists(USERS_FILE))
            {
                var usersJson = File.ReadAllText(USERS_FILE);
                Collections.Users = JsonConvert.DeserializeObject<ConcurrentDictionary<int, User>>(usersJson);
            }
            if (File.Exists(VIRTUAL_SERVERS_FILE))
            {
                var serversJson = File.ReadAllText(VIRTUAL_SERVERS_FILE);
                Collections.Users = JsonConvert.DeserializeObject<ConcurrentDictionary<int, User>>(serversJson);
            }
        }

        public bool UserExists(User user) => Collections.Users.ContainsKey(user.Id);
        public bool AddUser(User user) => Collections.Users.TryAdd(user.Id, user);
        public bool Authenticate(ref User user)
        {
            if (UserExists(user))
            {
                var dbUser = Collections.Users[user.Id];
                if (dbUser.Username == user.Username)
                {
                    if (dbUser.Password == user.Password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetNextUserId()
        {
            return Collections.Users.Count + 1;
        }

        public int GetNextServerId()
        {
            return Collections.VirtualServers.Count + 1;
        }

        public void Save()
        {
            var userJson = JsonConvert.SerializeObject(Collections.Users);
            var serverJson = JsonConvert.SerializeObject(Collections.VirtualServers);
            File.WriteAllText(USERS_FILE,userJson);
            File.WriteAllText(VIRTUAL_SERVERS_FILE,serverJson);
        }
    }
}
