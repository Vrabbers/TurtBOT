using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace TurtBOT.CommandModules
{
    public class MainModule : ModuleBase
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
            await task.ModifyAsync(msg => msg.Content = $"Ping with {watch.ElapsedMilliseconds}ms!");
        }

        [Command("help")]
        [Summary("Gets help")]
        public async Task Help([Summary("Get help for")]string name = null)
        {
            var embedb = new EmbedBuilder();
            if (name == null)
            {
                string help = default;
                foreach (var c in CommandService.Commands)
                {
                    help += c.Name + "\n";
                }
                embedb.WithTitle("Help")
                    .WithDescription(help);
                await ReplyAsync(embed: embedb.Build());
            }
            else
            {
                var cmd = CommandService.Commands.First(c => c.Name == name);
                var usageString = $"{Bot.BotConfig.Prefix}{cmd.Name} ";
                var usageFields = new List<EmbedFieldBuilder>();
                for (int i = 0; i < cmd.Parameters.Count; i++ )
                {
                    var parameter = cmd.Parameters[i];
                    usageString += parameter.IsOptional ? $"({parameter.Name})" : parameter.Name;
                    usageFields.Add(new EmbedFieldBuilder()
                        .WithName($"{Utils.TypeName(parameter.Type)} {parameter.Name}")
                        .WithValue(parameter.Summary)
                        .WithIsInline(true));
                }
                embedb.WithTitle(cmd.Name)
                    .WithDescription(cmd.Summary)
                    .WithFields(new EmbedFieldBuilder().WithName("Usage").WithValue(usageString))
                    .WithFields(usageFields);
                await ReplyAsync(embed: embedb.Build());
            }
        }

        [Command("test")]
        [Summary("This command has many arguments")]
        public async Task Test(
            [Name("1st")] [Summary("The 1st one")] string a,
            [Name("2nd")] [Summary("The 2nd one")] string b,
            [Name("3rd")] [Summary("The 3rd one")] int c)
        {
            await ReplyAsync(a + b + c);
        }
    }
}