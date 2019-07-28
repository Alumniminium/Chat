using System;

namespace Client.UI.Commands
{
    public static class ServerCommandHandler
    {
        public static void Process(string input)
        {
            var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (args.Length == 2)
            {
                SetServer(args);
            }

            if (args.Length == 3)
            {
                SetServer(args);
                SetChannel(args);
            }
        }

        private static void SetServer(string[] args)
        {
            var serverName = args[1];

            foreach (var kvp in Core.MyUser.Servers)
            {
                if (kvp.Value.Name.Contains(serverName, StringComparison.InvariantCultureIgnoreCase))
                {
                    Core.SelectedServer = kvp.Value;
                    Console.WriteLine($"Switched to '{kvp.Value.Name}' Server");
                    break;
                }
            }
        }
        private static void SetChannel(string[] args)
        {
            var serverName = args[1];
            var channelName = args[2];

            foreach (var kvp in Core.MyUser.Servers)
            {
                if (kvp.Value.Name.Contains(serverName, StringComparison.InvariantCultureIgnoreCase))
                {
                    Core.SelectedServer = kvp.Value;
                    foreach (var channel in Core.SelectedServer.Channels.Values)
                    {
                        if (channel.Name.Contains(channelName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            Core.SelectedChannel = channel;
                            Console.WriteLine($"Switched to '{kvp.Value.Name}' Server - Channel '{channel.Name}'");
                            break;
                        }
                    }
                }
            }
        }
    }
}