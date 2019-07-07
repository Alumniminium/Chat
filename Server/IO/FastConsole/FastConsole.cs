using System;

namespace Client.IO.FastConsole
{
    public static class FastConsole
    {
        public static string Title { get => Console.Title; set=>Console.Title=value;}

        public static void WriteLine(string msg)
        {
            FastConsoleThread.Add(msg);
        }
        public static void WriteLine(object msg)
        {
            FastConsoleThread.Add(msg.ToString());
        }

        public static void WriteLine(Exception msg)
        {
            FastConsoleThread.Add(msg.ToString());
        }

        public static string ReadLine() => Console.ReadLine();
    }
}