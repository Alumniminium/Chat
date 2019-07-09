using Universal.IO.FastConsole;
using Universal.Packets;
using Universal.Packets.Enums;

namespace Client.Networking.Handlers
{
    public static class MsgDataRequestHandler
    {
        public static void Process(MsgDataRequest msgDataRequest)
        {

            switch (msgDataRequest.Type)
            {
                case MsgDataRequestType.Friends:
                    {
                        FConsole.WriteLine("Stage Completed: Sync Friendlist");

                        foreach (var friend in Core.MyUser.Friends)
                        {
                            var request = MsgDataRequest.CreateRequestMissedMessagesPacket(Core.MyUser.Id, friend.Key);
                            Core.Client.Send(request);
                        }

                        var request2 = MsgDataRequest.CreateServerListRequest(Core.MyUser.Id);
                        Core.Client.Send(request2);
                        break;
                    }

                case MsgDataRequestType.VServers:
                    {
                        FConsole.WriteLine("Stage Completed: Sync VServers");
                        foreach (var server in Core.MyUser.Servers)
                        {
                            var request = MsgDataRequest.CreateServerChannelListRequest(Core.MyUser.Id, server.Key);
                            Core.Client.Send(request);
                        }
                        break;
                    }

                case MsgDataRequestType.Channels:
                    FConsole.WriteLine("Stage Completed: Sync Channels of " + Core.MyUser.Servers[msgDataRequest.TargetId].Name);
                    foreach (var server in Core.MyUser.Servers.Values)
                    {
                        foreach (var channel in server.Channels)
                        {
                            var request = MsgDataRequest.CreateRequestMissedMessagesPacket(Core.MyUser.Id, server.Id, channel.Key);
                            Core.Client.Send(request);
                        }
                    }
                    break;
                case MsgDataRequestType.Messages:
                    FConsole.WriteLine("Stage Completed: Sync Messages of " + Core.MyUser.Servers[msgDataRequest.TargetId].Channels[msgDataRequest.Param].Name);
                    break;
                default:
                    FConsole.WriteLine("Invalid stage.");
                    break;
            }

        }
    }
}