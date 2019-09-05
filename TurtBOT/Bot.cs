using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TurtBOT
{
    public static class Bot
    {
        public static BotConfig BotConfig;

        private static DateTime startTime;
        
        private static readonly DiscordSocketClient Client = new DiscordSocketClient();

        private static readonly CommandService Commands = new CommandService();
        
        public static async Task Initialize(string token, BotConfig config)
        {
            BotConfig = config;
            Client.Log += Log;
            Client.MessageReceived += OnMessageReceived;
            
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
            startTime = DateTime.Now;
        }

        public static async Task ReInit(BotConfig config)
        {
            await Client.StopAsync();
            BotConfig = config;
            await Client.StartAsync();
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
            if (msgpar is SocketUserMessage msg && msg.Content.StartsWith(BotConfig.Prefix) && !msg.Author.IsBot)
            {
                var c = await Commands.ExecuteAsync(new SocketCommandContext(Client, msg), BotConfig.Prefix.Length, null);
                if (!c.IsSuccess)
                {
                    await msg.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithTitle("<:blobexplosion:516363170072231936> Error!")
                        .WithDescription($"**{c.Error}**: {c.ErrorReason}")
                        .WithColor(Color.Red)
                        .WithFooter("This problem has been reported. If you don't understand what this means, don't worry.")
                        .Build());
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error in Command '{0}': {1} {2}", msg.Content, c.Error, c.ErrorReason);
                    Console.ResetColor();
                }
            }
        }

        public static TimeSpan GetUptime()
        {
            return DateTime.Now - startTime;
        }
    }
}