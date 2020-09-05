using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TurtBOT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = TokenSetup();
            var config = BotConfig.Setup();

            var client = new DiscordSocketClient();
            client.Log += Client_Log;

            var commandHandler = new CommandHandler(client, new CommandService(), config);
            await commandHandler.InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            while (true) Console.ReadLine();
        }

        static Task Client_Log(LogMessage msg)
        {
            Console.ForegroundColor = msg.Severity switch
            {
                LogSeverity.Error => ConsoleColor.Red,
                LogSeverity.Warning => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
            Console.WriteLine($"[{msg.Severity} {DateTime.Now:T}] {msg.Message}");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        static string TokenSetup()
        {
            if (!File.Exists("token.txt"))
            {
                Console.Write("Please insert your bot account's token: ");
                var token = Console.ReadLine();
                using var file = File.CreateText("token.txt");
                file.Write(token);
                return token;
            }
            
            return File.ReadAllText("token.txt"); 
        }
    }
}
