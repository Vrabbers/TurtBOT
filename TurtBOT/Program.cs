using System;
using System.IO;
using System.Threading.Tasks;

namespace TurtBOT
{
    class Program
    {
        
        public static async Task Main(string[] args)
        {
            string token;
            if (!File.Exists("token.txt"))
            {
                Console.Write("Please insert your bot account's token:");
                token = Console.ReadLine();
                await using var file = File.CreateText("token.txt");
                file.Write(token);
            }
            token = File.ReadAllText("token.txt");
            var config = new BotConfig(){Prefix = "tb:"};
            Bot.Initialize(token,config);
            Console.ReadLine();
        }

    }
}