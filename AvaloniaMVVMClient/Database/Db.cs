using System.IO;
using Newtonsoft.Json;

namespace AvaloniaMVVMClient.Database
{
    public static class Db
    {
        public static void LoadConfig()
        {
            if (File.Exists("config.json"))
                Core.Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        }

        public static void SaveConfig()
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(Core.Config));
        }
    }
}
