using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.EntityFrameworkCore;

namespace TurtBOT.CommandModules
{
    public class PublicModule : ModuleBase
    {
        public CommandService CommandService { get; set; }

        [Command("ping")]
        [Summary("Pings the bot")]
        public async Task Ping()
        {
            var watch = new Stopwatch();
            watch.Start();
            var task = await ReplyAsync("pinging");
            watch.Stop();
            await task.ModifyAsync(msg => msg.Content = $"Hello {Context.User.Username}Ping with {watch.ElapsedMilliseconds}ms!");
        }
        
        [Command("getnick")]
        [Summary("Pings the bot")]
        public async Task GetNick()
        {
            var user = await DbHandler.GetUser(Context.User.Id);
            await ReplyAsync(user.DefinedNickname is null
                ? "you dotn have nick name"
                : $"nick name is {user.DefinedNickname}");
        }

        [Command("setnick")]
        [Summary("Pings the bot")]
        public async Task SetNick(string n)
        {
            await DbHandler.ChangeUserNickname(Context.User.Id, n);
        }
        
        [Command("help")]
        [Summary("Gets help")]
        public async Task Help([Summary("Command to get extra help for")]string name = null)
        {
            var embedBuilder = new EmbedBuilder();
            if (name == null)
            {
                embedBuilder.WithTitle("Help").WithColor(Color.Blue);
                var mainHelp= "";
                var modHelp = "";
                var ownerHelp = "";

                foreach (var c in CommandService.Commands)
                {
                    if (c.Preconditions.Count == 0)
                        mainHelp += $"{c.Name}\n";
                    else if (c.Preconditions.OfType<RequireOwnerAttribute>().Count() != 0)
                        ownerHelp += $"{c.Name}\n";
                    else
                        modHelp += $"{c.Name}\n";
                    
                }

                embedBuilder.WithFields(new EmbedFieldBuilder()
                    .WithName("Public")
                    .WithValue(mainHelp)
                    .WithIsInline(true));
                if (modHelp != "")
                {
                    embedBuilder.WithFields(new EmbedFieldBuilder()
                        .WithName("Mod")
                        .WithValue(modHelp)
                        .WithIsInline(true));
                }
                if(ownerHelp != "")
                {
                    embedBuilder.WithFields(new EmbedFieldBuilder()
                        .WithName("Owner")
                        .WithValue(ownerHelp)
                        .WithIsInline(true));
                }
                await ReplyAsync(embed: embedBuilder.Build());
            }
            else
            {
                var cmd = CommandService.Commands.First(c => c.Name == name);
                var usageString = $"{Bot.BotConfig.Prefix}{cmd.Name} ";
                var usageFields = new List<EmbedFieldBuilder>();
                foreach (var parameter in cmd.Parameters)
                {
                    usageString += parameter.IsOptional ? $"({parameter.Name}) " : $"{parameter.Name} ";
                    usageFields.Add(new EmbedFieldBuilder()
                        .WithName($"{Utils.TypeName(parameter.Type)} {parameter.Name}")
                        .WithValue(parameter.Summary)
                        .WithIsInline(true));
                }
                embedBuilder.WithTitle(cmd.Name)
                    .WithDescription(cmd.Summary)
                    .WithFields(new EmbedFieldBuilder().WithName("Usage").WithValue(usageString))
                    .WithColor(Color.Green)
                    .WithFields(usageFields);
                if(cmd.Parameters.Count != 0) { embedBuilder.WithFooter("Parameters in (parentheses) are optional.");}
                await ReplyAsync(embed: embedBuilder.Build());
            }
        }

        [Command("uptime")]
        [Summary("Gets the uptime")]
        public async Task Uptime()
        {
            var formattedTime = Bot.GetUptime().ToString(@"hh\:mm\:ss");
            await ReplyAsync(embed: new EmbedBuilder()
                .WithTitle("Uptime")
                .WithColor(Color.Blue)
                .WithDescription($"I've been up for {formattedTime}")
                .Build());
        }
    }
}