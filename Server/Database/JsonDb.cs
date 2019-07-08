using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore.Internal;
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
                Collections.Users = JsonConvert.DeserializeObject<ConcurrentDictionary<int, User>>(serversJson);
            }

            CreateDemoData();
        }

        private void CreateDemoData()
        {
            if (Collections.VirtualServers.Count != 0)
                return;

            var demoServer = new VirtualServer {Id = GetNextServerId(), Name = "Demo Server", OwnerId = 0};
            var channels = new Channel[2];

            for (var i = 0; i < channels.Length; i++)
            {
                channels[i] = new Channel {Id = i, Name = "Demo Channel " + i, Messages = new List<Message>()};
                var message = new Message {Id = i, AuthorId = 0, Timestamp = DateTime.Now, Text = "Hello World!"};

                channels[i].Messages.Add(message);
                demoServer.Channels.Add(channels[i].Id, channels[i]);
            }

            var demoUser = new User {Id = GetNextUserId(), Username = "demo", Password = "demo"};

            demoUser.VirtualServers.Add(demoServer.Id);
            demoServer.Members.Add(demoUser.Id);

            Collections.Users.TryAdd(demoUser.Id, demoUser);
            Collections.VirtualServers.TryAdd(demoServer.Id, demoServer);

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

        public void LoadUser(User user)
        {
            var dbUser = GetDbUser(user.Username);

            user.Id = dbUser.Id;
            user.Friends = dbUser.Friends;
            user.VirtualServers = dbUser.VirtualServers;
            user.Socket.StateObject = user;
            user.Socket = user.Socket;
        }


        public int GetNextUserId() => Collections.Users.Count + 1;
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
