using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TurtBOT
{
    class Program
    {
           
        public static async Task Main(string[] args)
        {

            string token;
            BotConfig config;
            if (!File.Exists("token.txt"))
            {
                Console.Write("Please insert your bot account's token:");
                token = Console.ReadLine();
                await using var file = File.CreateText("token.txt");
                file.Write(token);
            }
            token = File.ReadAllText("token.txt");

            if (!File.Exists("config.json"))
            {
                config = Config();
                var configJson = JsonConvert.SerializeObject(config);
                await using var file = File.CreateText("config.json");
                file.Write(configJson);
            }

            try
            {
                config = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("config.json"));
            }
            catch (JsonSerializationException e)
            {
                config = Config();
                var configJson = JsonConvert.SerializeObject(config);
                await using var file = File.CreateText("config.json");
                file.Write(configJson);
            }

            await Bot.Initialize(token,config);
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "config":
                        config = Config(config);
                        
                        var configJson = JsonConvert.SerializeObject(config);
                        await using (var file = File.CreateText("config.json"))
                        {
                            file.Write(configJson);
                        }
                        
                        await Bot.ReInit(config);
                        break;
                    case "exit":
                        return;
                }
            }

            
        }

        static BotConfig Config(BotConfig bc = new BotConfig())
        {
            Console.Write("Prefix [{0}]:", bc.Prefix);
            var pref = Console.ReadLine();
            if (pref != "") bc.Prefix = pref;
            Console.Write("Error Message[{0}]:", bc.ErrorMessage);
            var erms = Console.ReadLine();
            if (erms != "") bc.ErrorMessage = erms;
            return bc;
        }
    }
}