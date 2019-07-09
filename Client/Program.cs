using System;
using System.Diagnostics;
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
                FConsole.ReadLine();
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