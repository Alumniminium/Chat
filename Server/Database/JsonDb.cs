using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Server.Entities;

namespace Server.Database
{
    public class JsonDb
    {
        private static string VServersFile { get; set; }
        private static string UsersFile { get; set; }
        private static string SettingsFile { get; set; }

        public void EnsureDbReady()
        {
            var pathRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var programFolder = Path.Combine(pathRoot, "Chat");
            var serverFolder = Path.Combine(programFolder, "Server");

            Directory.CreateDirectory(programFolder);
            Directory.CreateDirectory(serverFolder);

            UsersFile = Path.Combine(serverFolder, "Users.json");
            VServersFile = Path.Combine(serverFolder, "VirtualServers.json");
            SettingsFile = Path.Combine(serverFolder, "Settings.json");

            if (File.Exists(SettingsFile))
            {
                var settingsJson = File.ReadAllText(SettingsFile);
                Core.Settings = JsonConvert.DeserializeObject<ServerSettings>(settingsJson);
            }
            if (File.Exists(UsersFile))
            {
                var usersJson = File.ReadAllText(UsersFile);
                Collections.Users = JsonConvert.DeserializeObject<ConcurrentDictionary<int, User>>(usersJson);
            }
            if (File.Exists(VServersFile))
            {
                var serversJson = File.ReadAllText(VServersFile);
                Collections.VirtualServers = JsonConvert.DeserializeObject<ConcurrentDictionary<int, VirtualServer>>(serversJson);
            }

            CreateDemoData();
        }

        private void CreateDemoData()
        {
            if (Collections.VirtualServers.Count != 0)
                return;

            var demoServer = new VirtualServer { Id = GetNextServerId(), Name = "C# Inn", OwnerId = 0, IconUrl = "http://i.epvpimg.com/MvbWaab.jpg" };
            Collections.VirtualServers.TryAdd(demoServer.Id, demoServer);
            var demoServer2 = new VirtualServer { Id = GetNextServerId(), Name = "Virtual Server", OwnerId = 0, IconUrl = "http://i.epvpimg.com/Pkargab.jpg" };
            Collections.VirtualServers.TryAdd(demoServer2.Id, demoServer2);
            var demoServer3 = new VirtualServer { Id = GetNextServerId(), Name = "Garbage Collectors", OwnerId = 0, IconUrl = "http://i.epvpimg.com/PKDRbab.jpg" };
            Collections.VirtualServers.TryAdd(demoServer3.Id, demoServer3);

            var channels = new Channel[10];
            channels[0] = new Channel(0, "# Welcome");
            channels[1] = new Channel(1, "# Rules");
            channels[2] = new Channel(2, "# Announcements");
            channels[3] = new Channel(3, "# General");
            channels[4] = new Channel(4, "# Random");
            channels[5] = new Channel(5, "# Questions");
            channels[6] = new Channel(6, "# Bots");
            channels[7] = new Channel(7, "# Offtopic");
            channels[8] = new Channel(8, "# Other Languages");
            channels[9] = new Channel(9, "# Partners");

            var m0 = new Message {Id = 0, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "Welcome to C# Inn!"};
            var m1 = new Message {Id = 1, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "Please read our #Rules"};
            channels[0].Messages.Add(m0.Id,m0);
            channels[0].Messages.Add(m1.Id, m1);

            var m2 = new Message
                {Id = 0, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "1. Don't be a cunt"};
            var m3 = new Message {Id = 1, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "2. Don't spam"};
            var m4 = new Message { Id = 2, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "3. No NSFW" };
            var m5 = new Message { Id = 3, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "4. See rule 1" };
            var m6 = new Message { Id = 0, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "WE HAVE ALUMNI CHAT NOW" };
            var m7 = new Message { Id = 1, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "Sadly it doesn't work." };
            var m8 = new Message {Id = 0, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "Sup neko?"};
            var m9 = new Message {Id = 1, AuthorId = 1_000_001, Timestamp = DateTime.UtcNow, Text = "Fuck off."};
            var m10 = new Message {Id = 2, AuthorId = 1_000_000, Timestamp = DateTime.UtcNow, Text = "Good talk."};

            channels[1].Messages.Add(m2.Id,m2);
            channels[1].Messages.Add(m3.Id,m3);
            channels[1].Messages.Add(m4.Id,m4);
            channels[1].Messages.Add(m5.Id,m5);

            channels[2].Messages.Add(m6.Id,m6);
            channels[2].Messages.Add(m7.Id,m7);

            channels[3].Messages.Add(m8.Id,m8);
            channels[3].Messages.Add(m9.Id, m9);
            channels[3].Messages.Add(m10.Id, m10);

            foreach (var channel in channels)
                demoServer.Channels.Add(channel.Id, channel);
            
            var demoUser = new User { Id = GetNextUserId(), Username = "demo", Password = "demo", Nickname = "Alumni", AvatarUrl = "https://i.epvpimg.com/MvbWaab.jpg" };
            Collections.Users.TryAdd(demoUser.Id, demoUser);
            var demoUser2 = new User { Id = GetNextUserId(), Username = "demo2", Password = "demo2", Nickname = "Neko", AvatarUrl = "http://i.epvpimg.com/Pkargab.jpg" };
            Collections.Users.TryAdd(demoUser2.Id, demoUser2);
            var demoUser3 = new User { Id = GetNextUserId(), Username = "demo3", Password = "demo3", Nickname = "Julian", AvatarUrl = "http://i.epvpimg.com/PKDRbab.jpg" };
            Collections.Users.TryAdd(demoUser3.Id, demoUser3);

            demoUser.VirtualServers.Add(demoServer.Id);
            demoUser.VirtualServers.Add(demoServer2.Id);
            demoUser.VirtualServers.Add(demoServer3.Id);

            demoServer.Members.Add(demoUser.Id);
            demoServer.Members.Add(demoUser2.Id);
            demoServer.Members.Add(demoUser3.Id);

            demoUser.Friends.Add(demoUser3.Id);
            demoUser.Friends.Add(demoUser2.Id);

            demoUser2.Friends.Add(demoUser3.Id);
            demoUser2.Friends.Add(demoUser.Id);

            demoUser3.Friends.Add(demoUser.Id);
            demoUser3.Friends.Add(demoUser2.Id);

            var defaultSettings = new ServerSettings
            {
                Port = 65535,
                PerUserCreateServerAllowance = 3,
                MaxMessagesPerUserPerSecond = 3,
                MaxLoginAttempts = 3,
                AutobanDurationSeconds = 5,
                DeleteInactiveServersAfter = TimeSpan.FromDays(356),
                DeleteInactiveUsersAfter = TimeSpan.FromDays(180)
            };

            Core.Settings = defaultSettings;
        }

        public bool UserExists(string user) => Collections.Users.Values.Any(u => u.Username == user);
        public bool AddUser(User user) => Collections.Users.TryAdd(user.Id, user);
        public bool Authenticate(string username, string password) => Collections.Users.Values.Any(u => u.Username == username && u.Password == password);
        public User GetDbUser(string username) => Collections.Users.Values.First(u => u.Username == username);

        public void LoadUser(ref User user)
        {
            var dbUser = GetDbUser(user.Username);
            dbUser.Socket = user.Socket;
            dbUser.Socket.StateObject = dbUser;
            user = dbUser;
        }


        public int GetNextUserId() => Collections.Users.Count + 1_000_000;
        public int GetNextServerId() => Collections.VirtualServers.Count + 1;

        public void Save()
        {
            var userJson = JsonConvert.SerializeObject(Collections.Users, Formatting.Indented);
            var serverJson = JsonConvert.SerializeObject(Collections.VirtualServers, Formatting.Indented);
            var settingsJson = JsonConvert.SerializeObject(Core.Settings, Formatting.Indented);
            File.WriteAllText(UsersFile, userJson);
            File.WriteAllText(VServersFile, serverJson);
            File.WriteAllText(SettingsFile, settingsJson);
        }
    }
}
