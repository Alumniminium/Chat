using System;

namespace Client.UI.Commands
{
    public static class ChannelCommandHandler
    {
        public static void Process(string input)
        {
            var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (args.Length == 2)
            {
                var channelName = args[1];

                foreach (var channel in Core.SelectedServer.Channels.Values)
                {
                    if (channel.Name.Contains(channelName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Core.SelectedChannel = channel;
                        break;
                    }
                }
            }
        }
    }
}