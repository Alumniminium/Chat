using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Universal.IO.FastConsole;

namespace Client
{
    public static class Program
    {

        public static void Main()
        {
            FConsole.Title = "CLIENT APP";
            SetupCountermeasuresForShitCode();
            Core.Client.ConnectAsync();
            
            while (true)
            {
                DrawUI();
                FConsole.ReadLine();
            }
        }

        public static void DrawUI()
        {
            var user = Core.MyUser;
            Console.Clear();
            Console.WriteLine("------ S E R V E R S ------|----- F r i e n d s ------");

            int counter = 0;
            var friends = user.Friends.Values.ToArray();
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
                if(server.Channels.Count ==0)
                    Console.WriteLine($"| -   |--- No channels.");
            }

            Console.SetCursorPosition(0, Console.WindowHeight-1);
        }

        private static void SetupCountermeasuresForShitCode()
        {
            TaskScheduler.UnobservedTaskException += (_, exception) =>
            {
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.Message}");
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.StackTrace}");
                exception.SetObserved();
                Debugger.Break();
            };
            AppDomain.CurrentDomain.UnhandledException += (_, exception) =>
            {
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.Message}");
                FConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.StackTrace}");
                Debugger.Break();
            };
        }
    }
}