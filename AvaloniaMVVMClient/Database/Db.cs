using System;
using System.IO;
using System.Net;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Newtonsoft.Json;

namespace AvaloniaMVVMClient.Database
{
    public static class Db
    {
        public static void LoadConfig()
        {
            if (File.Exists("config.json"))
                Core.StateFile = JsonConvert.DeserializeObject<StateFile>(File.ReadAllText("config.json"));
        }

        public static void SaveConfig()
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(Core.StateFile, Formatting.Indented));
        }

        public static string GetCacheImage(string url)
        {
            if (Core.StateFile.Cache.TryGetValue(url, out var cachePath))
                return cachePath;
            using (var webclient = new WebClient())
            {
                cachePath = Path.Combine("file://", Environment.CurrentDirectory, "cache", Path.ChangeExtension(Path.GetRandomFileName(), "jpg"));
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(cachePath));
                webclient.DownloadFile(url, cachePath);
                Core.StateFile.Cache.TryAdd(url, cachePath);
                return cachePath;
            }
        }
    }
}
