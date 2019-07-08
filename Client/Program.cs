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

        public static async Task Main()
        {
            FastConsole.Title = "CLIENT APP";

            SetupCountermeasuresForShitCode();

            await LoadConfigAsync();

            Core.Client.ConnectAsync();

            while (true)
                FastConsole.ReadLine();
        }

        private static async Task LoadConfigAsync()
        {
            await Task.Run(() =>
            {
                if (File.Exists("config.json"))
                    Core.Client = JsonConvert.DeserializeObject<Client>(File.ReadAllText("config.json"));
                else
                    FastConsole.WriteLine("No config.json was found at " + Environment.CurrentDirectory + "/config.json");
            });
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