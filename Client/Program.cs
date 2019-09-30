using System;
using System.Linq;
using Client.UI;
using Client.UI.Commands;
using Universal.Exceptions;
using Universal.IO.FastConsole;
using Universal.Performance;

namespace Client
{
    public static class Program
    {

        public static void Main()
        {
            FConsole.Title = "CLIENT APP";
            GlobalExceptionHandler.Setup();
            Jit.PreJit();

            StartupScreen.Draw();

            while (true)
            {
                DrawUI();
                //HomeScreen.Draw();
                var input = FConsole.ReadLine();

                if (input.StartsWith('/'))
                {
                    var command = input.Split(' ')[0];
                    switch (command)
                    {
                        case "/s":
                            ServerCommandHandler.Process(input);
                            break;
                        case "/c":
                            ChannelCommandHandler.Process(input);
                            break;
                        case "/dm":
                            ServerCommandHandler.Process("/s 0");
                            break;
                    }
                }
                else
                {
                    Core.MyUser.SendMessage(input);
                }
            }
        }
        public static void DrawUI()
        {
            var user = Core.MyUser;
            Console.Clear();
            Console.WriteLine($"W: {Console.WindowWidth} H:{Console.WindowHeight}");
            Console.WriteLine("------ S E R V E R S ------|----- F r i e n d s ------");

            var counter = 0;
            var friends = user.Friends.Values.ToArray();

            try
            {
                foreach (var kvp in user.Servers)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"|--- {kvp.Value.Name}");
                    Console.SetCursorPosition(27, Console.CursorTop);
                    if (friends.Length > counter)
                        Console.Write($"|--- {friends[counter].Name}");
                    counter++;
                    Console.CursorTop++;
                }

                Console.SetCursorPosition(0, 6);
                foreach (var server in user.Servers.Values)
                {
                    Console.WriteLine($"|  - {server.Name} -");

                    if (Core.SelectedServer.Id != server.Id)
                        continue;

                    foreach (var (_, channel) in server.Channels)
                    {
                        Console.WriteLine($"|  -   |--- {channel.Name}");

                        if (Core.SelectedChannel.Id != channel.Id)
                            continue;

                        foreach (var (_, value) in channel.Messages)
                        {
                            Console.WriteLine($"|  -   |------- {Core.MyUser.GetFriend(value.AuthorId).Name} ---> {value.Text}");
                        }

                        if (channel.Messages.Count == 0)
                            Console.WriteLine($"|  -   |--- {channel.Name} ---> No new messages.");
                    }
                    if (server.Channels.Count == 0)
                        Console.WriteLine($"|  -   |--- No channels.");
                }
                Console.WriteLine();
            }
            catch (Exception e)
            {
                FConsole.WriteLine(e);
            }
        }
    }
}