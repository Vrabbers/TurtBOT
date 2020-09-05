using System;
using System.IO;
using System.Text.Json;

namespace TurtBOT
{
    public class BotConfig
    {
        public string BotPrefix { get; set; }
        public string ErrorMessage { get; set; }
        
        protected BotConfig() { }

        public static BotConfig Setup()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            BotConfig obj;

            if (File.Exists("config.json"))    
                obj = JsonSerializer.Deserialize<BotConfig>(File.ReadAllText("config.json"), options);
            else
                obj = new BotConfig();

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (!prop.CanWrite) continue;

                if (prop.GetValue(obj) is null)
                {
                    Console.Write($"Please input value for {prop.Name}: ");
                    var read = Console.ReadLine();

                    if (prop.PropertyType == typeof(string))
                        prop.SetValue(obj, read);
                }
            }

            File.WriteAllText("config.json", JsonSerializer.Serialize(obj, options));

            return obj;
        }
    }
}
