using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Universal.IO.FastConsole;

namespace Client
{
    public static class Program
    {

        public static void Main()
        {
            FastConsole.Title = "CLIENT APP";

            SetupCountermeasuresForShitCode();

            Core.Client.ConnectAsync();

            while (true)
                FastConsole.ReadLine();
        }
        private static void SetupCountermeasuresForShitCode()
        {
            TaskScheduler.UnobservedTaskException += (_, exception) =>
            {
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.Message}");
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {exception.Exception.StackTrace}");
                exception.SetObserved();
                Debugger.Break();
            };
            AppDomain.CurrentDomain.UnhandledException += (_, exception) =>
            {
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.Message}");
                FastConsole.WriteLine($"Congrats you idiot. Look what you did: {(exception.ExceptionObject as Exception)?.StackTrace}");
                Debugger.Break();
            };
        }
    }
}