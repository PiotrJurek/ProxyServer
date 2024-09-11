using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerLibrary.Config
{
    public static class ConfigLoader
    {
        public static Config LoadConfig(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Config file not found: {filePath}.");
            }

            string json = File.ReadAllText(filePath);
            Config? config = JsonSerializer.Deserialize<Config>(json);

            if(config == null)
            {
                throw new JsonException($"File was not in correct format.");
            }

            return config;
        }
    }
}
