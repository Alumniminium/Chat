using System;
using System.Collections.Generic;
using System.Text;

namespace Client.UI
{
    public static class LoginScreen
    {
        public static void Draw()
        {
            Console.Clear();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 3);
            Console.WriteLine("|▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒|");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 2);
            Console.WriteLine("|                                |");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 - 1);
            Console.Write("|  User: ");
            var user = Console.ReadLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2);
            Console.Write("|  Pass: ");
            var pass = Console.ReadLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 + 1);
            Console.WriteLine("|                                |");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 13, Console.WindowHeight / 2 + 2);
            Console.WriteLine("|________________________________|");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);

            Core.Client.ConnectAsync(user, pass);

            while (!Core.Client.Socket.IsConnected)
            {
                Console.ReadKey(true);
            }

            HomeScreen.Draw();
        }
    }
}
