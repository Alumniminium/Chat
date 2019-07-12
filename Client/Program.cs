using System;
using System.Linq;
using System.Threading;
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
            JIT.PreJIT();
            Core.Client.ConnectAsync();

            while (true)
            {
                Thread.Sleep(2000);
                DrawUI();
                FConsole.ReadLine();
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
                    Console.WriteLine($"| - {server.Name} -");
                    foreach (var channel in server.Channels.Values)
                    {
                        foreach (var message in channel.Messages)
                        {
                            Console.WriteLine($"| -   |--- {channel.Name} ---> {Core.MyUser.GetFriend(message.AuthorId).Name} says: {message.Text}");
                        }

                        if (channel.Messages.Count == 0)
                            Console.WriteLine($"| -   |--- {channel.Name} ---> No new messages.");
                    }
                    if (server.Channels.Count == 0)
                        Console.WriteLine($"| -   |--- No channels.");
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