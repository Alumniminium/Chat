using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using Client.Entities;
using System.Collections.Generic;
using Client.Database.Entities;
using System.Runtime.InteropServices;
using System;

namespace Client.Database
{
    public class JsonDb : IDb
    {
        static string VServersFile { get; set; }
        static string UsersFile { get; set; }
        static string SettingsFile { get; set; }

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

        private static void CreateDemoData()
        {
            if (Collections.VirtualServers.Count == 0)
            {
                var demoServer = new VirtualServer();
                demoServer.Id = 0;
                demoServer.Name = "Demo Server";
                demoServer.OwnerId = 0;
                var channels = new Channel[2];
                for (var i = 0; i < channels.Length; i++)
                {
                    channels[i] = new Channel();
                    channels[i].Id = i;
                    channels[i].Name = "Demo Channel " + i;
                    channels[i].Messages = new List<Message>();
                    
                    var message = new Message();
                    message.Id=i;
                    message.AuthorId=0;
                    message.Timestamp = DateTime.Now;
                    message.Text = "Hello World!";

                    channels[i].Messages.Add(message);


                    demoServer.Channels.Add(channels[i].Id, channels[i]);
                }

                var demoUser = new User();
                demoUser.Id = 0;
                demoUser.Username = "demo";
                demoUser.Password = "demo";
                demoUser.VirtualServers.Add(demoServer.Id);

                demoServer.Members.Add(demoUser.Id);

                Collections.Users.TryAdd(demoUser.Id, demoUser);
                Collections.VirtualServers.TryAdd(demoServer.Id, demoServer);

                var defaultSettings = new ServerSettings();
                defaultSettings.Port = 65535;
                defaultSettings.PerUserCreateServerAllowance = 3;
                defaultSettings.MaxMessagesPerUserPerSecond = 3;
                defaultSettings.MaxLoginAttempts = 3;
                defaultSettings.AutobanDurationSeconds = 5;
                defaultSettings.DeleteInactiveServersAfter = TimeSpan.FromDays(356);
                defaultSettings.DeleteInactiveUsersAfter = TimeSpan.FromDays(180);

                Core.Settings = defaultSettings;
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
            var userJson = JsonConvert.SerializeObject(Collections.Users, Formatting.Indented);
            var serverJson = JsonConvert.SerializeObject(Collections.VirtualServers, Formatting.Indented);
            var settingsJson = JsonConvert.SerializeObject(Core.Settings, Formatting.Indented);
            File.WriteAllText(UsersFile, userJson);
            File.WriteAllText(VServersFile, serverJson);
            File.WriteAllText(SettingsFile, settingsJson);
        }
    }
}
