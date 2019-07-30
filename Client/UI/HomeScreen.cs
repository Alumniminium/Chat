using System;

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

            ConsoleUi.DrawRectangle(0, 0, Console.WindowWidth, Console.WindowHeight);
            ConsoleUi.DrawRectangle(4, 2, halfwidth, halfheight);
        }
    }
}
