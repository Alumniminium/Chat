using System;
using System.Linq;
using Universal.Extensions;

namespace Client.UI
{
    public static class HomeScreen
    {
        public static void Draw()
        {
            Console.Clear();
<<<<<<< HEAD
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var halfwidth = width / 2;
            var halfheight = height / 2;

            ConsoleUI.DrawRectangle(0, 0, Console.WindowWidth, Console.WindowHeight);
            ConsoleUI.DrawRectangle(4, 2, 10, 5);


=======
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
>>>>>>> 638be578ec3e61e715fd972057ed899d8e32184a
        }
    }
}
