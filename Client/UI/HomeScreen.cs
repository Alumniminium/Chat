using System;
using System.Linq;
using Universal.Extensions;

namespace Client.UI
{
    public static class HomeScreen
    {
        public static void Draw()
        {
            var serverArray = Core.MyUser.Servers.Values.ToArray();
            var friendArray = Core.MyUser.Friends.Values.ToArray();
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 -25, 1);
            Console.WriteLine("|▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒|");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 2);
            Console.WriteLine("|           SERVERS      |       FRIENDS          |");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 3);
            Console.WriteLine("|_________________________________________________|");
            for (int i = 0; i < Math.Min(3,serverArray.Length); i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 4+i);
                Console.WriteLine($"| 1. {serverArray[0]?.Name?.CenterLength(20)}| 1. {friendArray[0]?.Name?.CenterLength(20)}|");
            }
        }
    }
}
