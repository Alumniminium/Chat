using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    public static class StartupScreen
    {
        public static void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 3);
            Console.WriteLine("|▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒|");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 2);
            Console.WriteLine("|                                |");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2-1);
            Console.WriteLine("|     (L)ogin or (R)egister?     |");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2);
            Console.WriteLine("|                                |");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2+1);
            Console.WriteLine("|________________________________|");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("#: ");

            var input = Console.ReadLine();

            if (input.ToUpper() == "L")
            {
                LoginScreen.Draw();
            }
            else if (input.ToUpper() == "R")
            {
                RegisterScreen.Draw();
            }
            else
            {
                Draw();
            }
        }
    }
}
