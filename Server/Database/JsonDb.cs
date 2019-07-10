using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Server.Entities;

namespace Server.Database
{
    public class JsonDb : IDb
    {
        private static string VServersFile { get; set; }
        private static string UsersFile { get; set; }
        private static string SettingsFile { get; set; }

        public void EnsureDbReady()
        {
            //	pathRoot = ApplicationData Windows = C:\Users\PLOPKOEK\AppData\Roaming\
            //	pathRoot = ApplicationData Linux   = ~/.config/
            var pathRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var programFolder = Path.Combine(pathRoot, "Chat");
            var serverFolder = Path.Combine(programFolder, "Server");

            Directory.CreateDirectory(programFolder);
            Directory.CreateDirectory(serverFolder);

            // serverFolder Windows = C:\Users\PLOPKOEK\AppData\Roaming\Chat\Server\
            // serverFolder Linux   = ~/.config/Chat/Server/
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

            var demoServer = new VirtualServer { Id = GetNextServerId(), Name = "Virtual Server", OwnerId = 0, IconUrl = "http://h.img.alumni.re/img/1.jpg" };
            Collections.VirtualServers.TryAdd(demoServer.Id, demoServer);
            var demoServer2 = new VirtualServer { Id = GetNextServerId(), Name = "C# Inn", OwnerId = 0, IconUrl = "http://h.img.alumni.re/img/2.jpg" };
            Collections.VirtualServers.TryAdd(demoServer2.Id, demoServer2);
            var demoServer3 = new VirtualServer { Id = GetNextServerId(), Name = "Garbage Collectors", OwnerId = 0, IconUrl = "http://h.img.alumni.re/img/3.jpg" };
            Collections.VirtualServers.TryAdd(demoServer3.Id, demoServer3);

            var channels = new Channel[2];

            for (var i = 0; i < channels.Length; i++)
            {
                channels[i] = new Channel {Id = i, Name = "Demo Channel " + i, Messages = new List<Message>()};
                var message = new Message {Id = i, AuthorId = 1_000_000+i, Timestamp = DateTime.UtcNow, Text = "Hello World!"};

                channels[i].Messages.Add(message);
                demoServer.Channels.Add(channels[i].Id, channels[i]);
            }

            var demoUser = new User { Id = GetNextUserId(), Username = "demo", Password = "demo", Nickname = "Alumni", AvatarUrl = "http://h.img.alumni.re/img/1.jpg"};
            Collections.Users.TryAdd(demoUser.Id, demoUser);
            var demoUser2 = new User { Id = GetNextUserId(), Username = "demo2", Password = "demo2", Nickname = "Neko", AvatarUrl = "http://h.img.alumni.re/img/2.jpg" };
            Collections.Users.TryAdd(demoUser2.Id, demoUser2);
            var demoUser3 = new User { Id = GetNextUserId(), Username = "demo3", Password = "demo3",Nickname = "Julian", AvatarUrl = "http://h.img.alumni.re/img/3.jpg" };
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

            Collections.Users.TryAdd(demoUser.Id, demoUser);

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
        public bool Authenticate(string username,string password) => Collections.Users.Values.Any(u => u.Username == username && u.Password == password);
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
