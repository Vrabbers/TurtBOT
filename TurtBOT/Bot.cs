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
        
        private static readonly DiscordSocketClient client = new DiscordSocketClient();

        private static readonly CommandService Commands = new CommandService();

        public static async Task Initialize(string token, BotConfig config)
        {
            BotConfig = config;
            client.Log += Log;
            client.MessageReceived += OnMessageReceived;
            
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }

        public static async Task ReInit(BotConfig config)
        {
            await client.StopAsync();
            BotConfig = config;
            await client.StartAsync();
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
                var c = await Commands.ExecuteAsync(new SocketCommandContext(client, msg), BotConfig.Prefix.Length, null);
                if (!c.IsSuccess)
                {
                    await msg.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithTitle("<:blobexplosion:516363170072231936> Error!")
                        .WithDescription(c.ErrorReason)
                        .WithColor(Color.Red).Build());
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error in Command '{0}': {1}", msg.Content, c.ErrorReason);
                    Console.ResetColor();
                }
            }
        }
    }
}