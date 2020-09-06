using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TurtBOT
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly BotConfig config;
        private readonly IServiceProvider services;

        public CommandHandler(DiscordSocketClient client, CommandService commands, BotConfig config)
        {
            this.commands = commands;
            this.client = client;
            this.config = config;

            services = new ServiceCollection()
                .AddSingleton(config)
                .BuildServiceProvider();
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;

            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            if (!(messageParam is SocketUserMessage message)) return;

            var argPos = 0;

            if (!(message.HasStringPrefix(config.BotPrefix, ref argPos)) || message.Author.IsBot) return;

            var context = new SocketCommandContext(client, message);

            var result = await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: services);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync($"{config.ErrorMessage} {result.ErrorReason}");
        }
    }
}
