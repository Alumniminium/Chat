using AvaloniaMVVMClient.Networking.Entities;
using AvaloniaMVVMClient.UI.ViewModels;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace AvaloniaMVVMClient.Networking.Networking.Handlers
{
    public static class MsgVServerHandler
    {
        public static void Process(byte[] buffer)
        {
            var msgVServer = (MsgVServer)buffer;

            var server = new VirtualServer
            {
                Id = msgVServer.UniqueId,
                Name = msgVServer.GetServerName(),
                IconUrl = msgVServer.GetServerIconUrl()
            };

            Core.MyUser.Servers.TryAdd(server.Id, server);
            (Core.Views[ViewModelEnum.Home].Item2 as HomeViewModel)?.Servers.Add(server);

            if (Core.SelectedServer == null)
                Core.SelectedServer = server;

            FConsole.WriteLine($"Received Server info for {server.Name}!");

            FConsole.WriteLine($"MsgVServer: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}
