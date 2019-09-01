using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TurtBOT
{
    class Program
    {
        private const string Prefix = "tb:";
        
        private static DiscordSocketClient client = new DiscordSocketClient();
        
        private static readonly CommandService commands = new CommandService();
        
        public IServiceProvider ServiceProvider { get; set; } = default!;

        public static async Task Main(string[] args)
        {
            client.Log += Log;
            client.MessageReceived += OnMessageReceived;
            string token;
            if (!File.Exists("token.txt"))
            {
                Console.WriteLine("Make a file called token.txt with the bot's token.");
                Environment.Exit(0);
            }
            token = File.ReadAllText("token.txt");

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
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

        static async Task OnMessageReceived(SocketMessage msgpar)
        {
            if (msgpar is SocketUserMessage msg && msg.Content.StartsWith(Prefix) && !msg.Author.IsBot)
            {
                await commands.ExecuteAsync(new SocketCommandContext(client, msg), Prefix.Length, null);
            }
        }
    }
}