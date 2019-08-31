using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TurtBOT
{
    class Program
    { 
        static DiscordSocketClient client = new DiscordSocketClient();
        static async Task Main(string[] args)
        {
            client.Log += Log;
            string token;
            if (!File.Exists("token.txt"))
            {
                Console.WriteLine("Make a file called token.txt with the bot's token.");
                Environment.Exit(0);
            }

            token = File.ReadAllText("token.txt");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        static Task Log(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg.ToString());
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(msg.ToString());
                    break;
                default:
                    Console.WriteLine(msg.ToString());
                    break;
            }
            Console.ResetColor();
            return Task.CompletedTask;
        }
    }
}