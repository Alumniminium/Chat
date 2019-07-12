using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 4);
            Console.WriteLine($"| 1. {serverArray[0].Name.CenterLength(20)}| 1. {friendArray[0].Name.CenterLength(20)}|");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 5);
            Console.WriteLine($"| 2. {serverArray[1].Name.CenterLength(20)}| 2. {friendArray[1].Name.CenterLength(20)}|");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 25, 6);
            Console.WriteLine($"| 3. {serverArray[2].Name.CenterLength(20)}|                        |");
        }
    }
}
