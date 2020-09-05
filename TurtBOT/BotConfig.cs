using System;
using System.Text.Json;

namespace TurtBOT
{
    public class BotConfig
    {
        public string Token { get; set; }
        public string BotPrefix { get; set; }
        public string ErrorMessage { get; set; }
        public string PostgreSqlConnectionString { get; set; }

        protected BotConfig() { }

        static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public static BotConfig Setup()
        {
            BotConfig obj;

            if (DataDirectory.Exists("config.json"))    
                obj = JsonSerializer.Deserialize<BotConfig>(DataDirectory.ReadString("config.json"), options);
            else
                obj = new BotConfig();

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (!prop.CanWrite) continue;

                if (prop.GetValue(obj) is null)
                {
                    Console.Write($"Please input value for {prop.Name}:");
                    var read = Console.ReadLine();

                    if (prop.PropertyType == typeof(string))
                        prop.SetValue(obj, read);
                }
            }

            DataDirectory.WriteString("config.json", JsonSerializer.Serialize(obj, options));

            return obj;
        }

        public static BotConfig NoSetup() => JsonSerializer.Deserialize<BotConfig>(DataDirectory.ReadString("config.json"), options);
    }
}
