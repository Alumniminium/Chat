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
            Console.Clear();
            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var halfwidth = width / 2;
            var halfheight = height / 2;

            ConsoleUI.DrawRectangle(0, 0, Console.WindowWidth, Console.WindowHeight);
            ConsoleUI.DrawRectangle(4, 2, 10, 5);


        }
    }
}
